using Feedro.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feedro.Model.Helper
{
    public static class QueryHelper
    {
        private static readonly char[] QuerySeparators = new char[] { ' ', '　', '\t' };

        private static int CalculateScore(Entry entry, string[] keywords)
        {
            Func <string, int> GetCountMatched = (text) =>
                {
                    return keywords.Where(word => text.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0).Count();
                };

            int scoreFromEntry = GetCountMatched(entry.Title) + GetCountMatched(entry.FormattedSummary);
            int scoreFromSubscription = GetCountMatched(entry.Subscription.Title);
            int boostFromBeingFavored = entry.IsFavorite ? 100 : 1;

            return (scoreFromEntry * 5 + scoreFromSubscription) * boostFromBeingFavored;
        }

        public static IEnumerable<Entry> SelectByKeywords(this IEnumerable<Entry> items, string query)
        {
            string[] keywords = (query ?? string.Empty).Split(QuerySeparators);

            return items
                .Select(item => new { Item = item, Score = CalculateScore(item, keywords) })
                .Where(scoredItem => scoredItem.Score > 0)
                .OrderByDescending(scoredItem => scoredItem.Score)
                .Select(scoredEntry => scoredEntry.Item);
        }
    }
}
