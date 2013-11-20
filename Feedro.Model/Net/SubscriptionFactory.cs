using Feedro.Model.Data;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Syndication;

namespace Feedro.Model.Net
{
    public class SubscriptionFactory
    {
        #region Constants

        public static readonly Regex RxLinkRss  = new Regex("<link[^>]* type=['\"]application/(rss|atom)\\+xml['\"][^>]*>");
        public static readonly Regex RxLinkIcon = new Regex("<link[^>]* rel=['\"]shortcut icon['\"][^>]*>");
        public static readonly Regex RxHref     = new Regex("href=['\"]([^\"]+)['\"]");

        #endregion

        private static async Task<string> GetHttpContentAsync(string uri)
        {
            HttpResponseMessage response = await new HttpClient().GetAsync(new Uri(uri));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        internal static string RetrieveSubscriptionUri(string uri, string content)
        {
            string retrievedUri = RxLinkRss.Matches(content)
                .Cast<Match>()
                .Select(match => RxHref.Match(match.Value))
                .Where(match => match.Success)
                .Select(match => match.Groups[1].Value)
                .FirstOrDefault();

            if (retrievedUri == null)
                throw new FormatException("Unable to find link tag for RSS feed.");

            Uri feedUri = null;

            // 絶対URI
            if (Uri.TryCreate(retrievedUri, UriKind.Absolute, out feedUri))
                return retrievedUri;

            // 相対URI
            if (Uri.TryCreate(new Uri(uri), retrievedUri, out feedUri))
                return feedUri.AbsoluteUri;

            throw new FormatException("Malformed URI");
        }

        internal static string RetrieveFaviconUri(string uri, string content)
        {
            string retrievedUri = RxLinkIcon.Matches(content)
                .Cast<Match>()
                .Select(match => RxHref.Match(match.Value))
                .Where(match => match.Success)
                .Select(match => match.Groups[1].Value)
                .FirstOrDefault();

            return retrievedUri ?? new Uri(new Uri(uri), "/favicon.ico").AbsoluteUri;
        }

        private static async Task<string> RetrieveSubscriptionTitle(string uri)
        {
            var client = new SyndicationClient();
            SyndicationFeed feed = await client.RetrieveFeedAsync(new Uri(uri));
            return feed.Title.Text;
        }

        public static async Task<Subscription> CreateSubscriptionAsync(string uri)
        {
            string content = await GetHttpContentAsync(uri);
            string feedUri = RetrieveSubscriptionUri(uri, content);
            string faviconUri = RetrieveFaviconUri(uri, content);
            string feedTitle = await RetrieveSubscriptionTitle(feedUri);

            return new Subscription()
            {
                Uri = feedUri,
                Title = feedTitle,
                FaviconUri = faviconUri
            };
        }
    }
}
