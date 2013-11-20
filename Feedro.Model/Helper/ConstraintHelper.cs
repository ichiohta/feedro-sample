using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feedro.Model.Data;

namespace Feedro.Model.Helper
{
    public static class ConstraintHelper
    {
        public static void VerifyUnique(this IEnumerable<Subscription> subscriptions, Subscription toTest)
        {
            if (subscriptions.SelectByUri(toTest.Uri) != null)
                throw new InvalidOperationException("Duplicate subscription");
        }

        public static void VerifySubscription(this IEnumerable<Entry> entries, IEnumerable<Subscription> subscriptions)
        {
            var subscrpitionUris = new HashSet<string>(
                subscriptions.Select(s => s.Uri),
                StringComparer.CurrentCultureIgnoreCase);

            Func<Entry, bool> IsOrphan = (e) =>
            {
                return e.Subscription == null ||
                    string.Compare(e.SubscriptionUri, e.Subscription.Uri, StringComparison.OrdinalIgnoreCase) != 0 ||
                    !subscrpitionUris.Contains(e.SubscriptionUri);
            };

            if (entries.Any(e => IsOrphan(e)))
                throw new InvalidOperationException("Entry with invalid subscription exists");
        }

        public static void VerifyUnique(this IEnumerable<Entry> entries, IEnumerable<Entry> toTest)
        {
            Func<Entry, string> toId = (e) => { return string.Format("{0}-{1}", e.DateTimePosted, e.Uri); };

            var existingIds = new HashSet<string>(
                entries.Select(e => toId(e)),
                StringComparer.OrdinalIgnoreCase);

            if (toTest.Any(e => existingIds.Contains(toId(e))))
                throw new InvalidOperationException("Duplicate entries");
        }
    }
}
