using Feedro.Model.Data;
using Feedro.Model.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feedro.Model.Net
{
    public class SubscriptionUpdater
    {
        #region Constants

        private const string SettingsLastChecked = "DateTimeChecked";
        private readonly DateTimeOffset DateTimeOffsetOrigin = default(DateTimeOffset);

        #endregion

        #region Properties

        public DateTimeOffset DateTimeChecked
        {
            get
            {
                return SettingHelper.GetLocalSetting(SettingsLastChecked, DateTimeOffsetOrigin);
            }
            set
            {
                SettingHelper.SetLocalSetting(SettingsLastChecked, value);
            }
        }

        #endregion

        #region Public methods

        public async Task<IEnumerable<Entry>> Update(Store store)
        {
            await store.Refresh();

            DateTimeOffset lastDatatimeChecked = DateTimeChecked;
            DateTimeChecked = DateTimeOffset.UtcNow;

            var result = new List<Entry>();

            foreach (Subscription subscription in store.Subscriptions)
            {
                IEnumerable<Entry> existing = store.Entries
                    .SelectBySubscription(subscription)
                    .ToArray();

                DateTimeOffset latest = existing.Any() ? existing.Max(entry => entry.DateTimePosted) : DateTimeOffsetOrigin;

                IEnumerable<Entry> downloaded = await EntryDownloader.RetrieveLatestEntries(subscription);

                IEnumerable<Entry> newEntries = downloaded
                    .Where(e => e.DateTimePosted > lastDatatimeChecked || e.DateTimePosted > latest)
                    .ToArray();

                store.AddEntries(newEntries);
                result.AddRange(newEntries);
            }

            await store.SaveChanges();

            return result;
        }

        #endregion
    }
}
