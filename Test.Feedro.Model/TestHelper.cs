using Feedro.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Test.Feedro.Model
{
    public static class TestDataFactory
    {

        public static Subscription NewTestSubscription()
        {
            string guid = Guid.NewGuid().ToString();

            return new Subscription()
            {
                Title = string.Format("title{0}", guid),
                Uri = string.Format("http://blogsite{0}.com/feed.xml", guid),
                FaviconUri = string.Format("http://blogsite{0}.com/favicon.ico", guid)
            };
        }

        public static Entry NewTestEntry(Subscription subscription)
        {
            string guid = Guid.NewGuid().ToString();

            return new Entry()
            {
                SubscriptionUri = subscription.Uri,
                Subscription = subscription,
                Uri = new Uri(new Uri(subscription.Uri), string.Format("/article{0}.html", guid)).AbsolutePath,
                DateTimePosted = DateTimeOffset.UtcNow,
                IsFavorite = false,
                Summary = string.Format("Summary of article {0} of blog {1}", guid, subscription.Title),
                Title = string.Format("Title of article {0} of blog {1}", guid, subscription.Title)
            };
        }
    }
}
