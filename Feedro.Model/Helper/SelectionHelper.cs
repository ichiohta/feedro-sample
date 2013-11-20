using Feedro.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feedro.Model.Helper
{
    public static class SelectionHelper
    {
        public static IEnumerable<Entry> SelectBySubscription(this IEnumerable<Entry> entries, Subscription subscription)
        {
            return entries.Where(e => string.Compare(e.SubscriptionUri, subscription.Uri, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public static Subscription SelectByUri(this IEnumerable<Subscription> subscriptions, string uri)
        {
            return subscriptions.SingleOrDefault(s => string.Compare(s.Uri, uri, StringComparison.OrdinalIgnoreCase) == 0);
        }
    }
}
