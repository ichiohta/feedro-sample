using Feedro.Model.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Feedro.Model.Data
{
    public class Store
    {
        #region Constants

        private const string Label_Subscriptions = "Feedro.Subscriptions";
        private const string Label_Entries = "Feedro.Entries";

        private const string SettingsDaysToKeep = "DaysToKeep";
        private const int DefaultSettingsDaysToKeep = 90;

        #endregion

        #region Helper types

        private class InvertedEntryComparer : IComparer<Entry>
        {
            public static InvertedEntryComparer Default = new InvertedEntryComparer();

            private InvertedEntryComparer()
            {
            }

            public int Compare(Entry x, Entry y)
            {
                return -Comparer<DateTimeOffset>.Default.Compare(x.DateTimePosted, y.DateTimePosted);
            }
        }

        #endregion

        #region Private variables to store property values

        private ObservableCollection<Subscription> _subscriptions;

        private ObservableCollection<Entry> _entries;

        #endregion

        #region Properties

        public ReadOnlyObservableCollection<Subscription> Subscriptions
        {
            get;
            private set;
        }

        public ReadOnlyObservableCollection<Entry> Entries
        {
            get;
            private set;
        }

        public int DaysToKeep
        {
            get
            {
                return SettingHelper.GetLocalSetting(SettingsDaysToKeep, DefaultSettingsDaysToKeep);
            }
            set
            {
                SettingHelper.SetLocalSetting(SettingsDaysToKeep, value);
            }
        }

        #endregion

        #region .ctor

        public Store()
        {
            _entries = new ObservableCollection<Entry>();
            Entries = new ReadOnlyObservableCollection<Entry>(_entries);

            _subscriptions = new ObservableCollection<Subscription>();
            Subscriptions = new ReadOnlyObservableCollection<Subscription>(_subscriptions);
        }

        #endregion

        #region Public methods

        public async Task SaveChanges()
        {
            await StorageHelper.Serialize<List<Subscription>>(Label_Subscriptions, _subscriptions.ToList());
            await StorageHelper.Serialize<List<Entry>>(Label_Entries, _entries.ToList());
        }

        public async Task Refresh()
        {
            IEnumerable<Subscription> loadedSubscriptions =
                await StorageHelper.Deserialize<List<Subscription>>(Label_Subscriptions, () => new List<Subscription>());

            IEnumerable<Entry> loadedEntries =
                await StorageHelper.Deserialize<List<Entry>>(Label_Entries, () => new List<Entry>());

            Dictionary<string, Subscription> uriToSubscriptions = loadedSubscriptions
                .ToDictionary(s => s.Uri, s => s, StringComparer.OrdinalIgnoreCase);

            foreach (Entry entry in loadedEntries)
                entry.Subscription = uriToSubscriptions[entry.SubscriptionUri];

            _subscriptions.Clear();
            _subscriptions.AddRange(loadedSubscriptions);

            DateTimeOffset dateTimeCutOff = DateTimeOffset.UtcNow.AddDays(-DaysToKeep);

            _entries.Clear();
            _entries.AddRange(loadedEntries.Where(entry => entry.DateTimePosted >= dateTimeCutOff));
        }

        public void AddSubscription(Subscription subscription)
        {
            _subscriptions.VerifyUnique(subscription);
            _subscriptions.Add(subscription);
        }

        public void RemoveSubscription(Subscription subscription)
        {
            Subscription toDelete = _subscriptions.SelectByUri(subscription.Uri);

            if (toDelete == null)
                return;

            _subscriptions.Remove(toDelete);

            IEnumerable<Entry> entriesToDelete = _entries.SelectBySubscription(subscription);

            _entries.RemoveRange(entriesToDelete);
        }

        public void AddEntries(IEnumerable<Entry> entriesToAdd)
        {
            entriesToAdd.VerifySubscription(Subscriptions);
            _entries.VerifyUnique(entriesToAdd);
            _entries.OrderdInsertRange(entriesToAdd.OrderByDescending(e => e.DateTimePosted), InvertedEntryComparer.Default);
        }

        #endregion
    }
}
