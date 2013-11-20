using Feedro.Model.Data;
using System.Collections.Generic;

namespace Test.Feedro.Model
{
    public class SubscriptionEqualityComparator : IEqualityComparer<Subscription>
    {
        public static SubscriptionEqualityComparator Default = new SubscriptionEqualityComparator();

        private SubscriptionEqualityComparator() { }

        public bool Equals(Subscription x, Subscription y)
        {
            if (x == y)
                return true;

            if (x == null || y == null)
                return false;

            return string.CompareOrdinal(x.Uri, y.Uri) == 0 &&
                string.CompareOrdinal(x.FaviconUri, y.FaviconUri) == 0 &&
                string.Compare(x.Title, y.Title) == 0;
        }

        public int GetHashCode(Subscription s)
        {
            return s.Uri.GetHashCode();
        }
    }

    public class EntryEqualityComparator : IEqualityComparer<Entry>
    {
        public static EntryEqualityComparator Default = new EntryEqualityComparator();

        private EntryEqualityComparator() { }

        public bool Equals(Entry x, Entry y)
        {
            if (x == y)
                return true;

            if (x == null || y == null)
                return false;

            return string.CompareOrdinal(x.Uri, y.Uri) == 0 &&
                string.CompareOrdinal(x.SubscriptionUri, y.SubscriptionUri) == 0 &&
                SubscriptionEqualityComparator.Default.Equals(x.Subscription, y.Subscription) &&
                string.Compare(x.Title, y.Title) == 0 &&
                x.DateTimePosted == y.DateTimePosted &&
                x.Summary == y.Summary &&
                x.IsFavorite == y.IsFavorite;
        }

        public int GetHashCode(Entry e)
        {
            return e.Uri.GetHashCode();
        }
    }
}
