using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feedro.Model.Data;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Test.Feedro.Model
{
    [TestClass]
    public class StoreTests
    {
        #region Helper methods

        private static Subscription NewTestSubscription()
        {
            string guid = Guid.NewGuid().ToString();

            return new Subscription()
            {
                Title = string.Format("title{0}", guid),
                Uri = string.Format("http://blogsite{0}.com/feed.xml", guid),
                FaviconUri = string.Format("http://blogsite{0}.com/favicon.ico", guid)
            };
        }

        private static Entry NewTestEntry(Subscription subscription)
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

        private static void AssertEquality(Subscription expected, Subscription actual)
        {
            Assert.AreEqual(expected.Uri, actual.Uri);
            Assert.AreEqual(expected.Title, actual.Title);
            Assert.AreEqual(expected.FaviconUri, actual.FaviconUri);
        }

        #endregion

        #region Test cases
        
        [TestMethod]
        public void AddSubscription_New()
        {
            var store = new Store();

            Subscription expected = NewTestSubscription();

            Assert.AreEqual(0, store.Subscriptions.Count());

            store.AddSubscription(expected);

            Assert.AreEqual(1, store.Subscriptions.Count());

            AssertEquality(expected, store.Subscriptions.First());
        }

        [TestMethod]
        public void AddSubscription_Duplicate()
        {
            var store = new Store();

            Subscription expected = NewTestSubscription();

            store.AddSubscription(expected);

            Assert.ThrowsException<InvalidOperationException>(
                () =>
                {
                    store.AddSubscription(expected);
                });
        }

        [TestMethod]
        public void RemoveSubscription_Existing()
        {
            var store = new Store();

            Subscription subscription = NewTestSubscription();

            store.AddSubscription(subscription);
            store.RemoveSubscription(subscription);

            Assert.AreEqual(0, store.Subscriptions.Count());
        }

        [TestMethod]
        public void RemoveSubscription_Nonexistent()
        {
            var store = new Store();

            Subscription subscription = NewTestSubscription();

            store.AddSubscription(subscription);
            store.RemoveSubscription(subscription);

            // should throw no exception
            store.RemoveSubscription(subscription);
        }

        [TestMethod]
        public void AddEntries_New()
        {
            var store = new Store();

            Subscription subscription = NewTestSubscription();

            store.AddSubscription(subscription);

            Entry expected = NewTestEntry(subscription);

            Assert.AreEqual(0, store.Entries.Count());

            store.AddEntries(new Entry[] { expected });

            Assert.AreEqual(1, store.Entries.Count());
        }

        [TestMethod]
        public void AddEntries_NoSubscription()
        {
            var store = new Store();

            Subscription subscription = NewTestSubscription();

            Entry entry = NewTestEntry(subscription);

            Assert.ThrowsException<InvalidOperationException>(
                () =>
                {
                    store.AddEntries(new Entry[] { entry });
                });
        }

        [TestMethod]
        public void RemoveSubscription_CascadeDeletion()
        {
            var store = new Store();

            Subscription subscription = NewTestSubscription();

            store.AddSubscription(subscription);

            Entry entry = NewTestEntry(subscription);

            store.AddEntries(new Entry[] { entry });

            store.RemoveSubscription(subscription);

            Assert.IsFalse(store.Entries.Any(e => e.SubscriptionUri == subscription.Uri));
        }

        [TestMethod]
        public async Task SaveChanges_Subscription()
        {
            var store = new Store();

            Subscription expected = NewTestSubscription();

            store.AddSubscription(expected);

            await store.SaveChanges();

            store = new Store();

            await store.Refresh();

            Subscription actual = store.Subscriptions.First();

            AssertEquality(expected, actual);
        }

        #endregion
    }
}
