using Feedro.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace Feedro.Model.Net
{
    public class EntryDownloader
    {
        internal static Entry NewEntryFrom(Subscription subscription, SyndicationItem item)
        {
            ISyndicationText title = item.Title;
            ISyndicationText summary = item.Summary;
            SyndicationLink link = item.Links.FirstOrDefault();
            ISyndicationNode node = item.ElementExtensions.FirstOrDefault(n => n.NodeName.Equals("updated"));

            DateTime dateTimePosted = item.LastUpdatedTime.DateTime;

            if (node != null)
                DateTime.TryParse(node.NodeValue, out dateTimePosted);

            return new Entry()
            {
                SubscriptionUri = subscription.Uri,
                Uri = link != null ? link.Uri.AbsoluteUri : null,
                Title = title != null ? title.Text : string.Empty,
                Summary = summary != null ? summary.Text : string.Empty,
                DateTimePosted = dateTimePosted,
                Subscription = subscription
            };
        }

        public static async Task<IEnumerable<Entry>> RetrieveLatestEntries(Subscription subscription)
        {
            var client = new SyndicationClient();
            SyndicationFeed feed = await client.RetrieveFeedAsync(new Uri(subscription.Uri));

            return feed
                .Items
                .Select(item => NewEntryFrom(subscription, item));
        }
    }
}
