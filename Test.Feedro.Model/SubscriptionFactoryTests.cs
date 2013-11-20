using Feedro.Model.Net;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Threading.Tasks;

namespace Test.Feedro.Model
{
    [TestClass]
    public class SubscriptionFactoryTests
    {
        [TestMethod]
        public void RetrieveSubscriptionUri_Uri_FindUri()
        {
            string actual = SubscriptionFactory.RetrieveSubscriptionUri(
                "http://foo.bar",
                "<html><head><link type=\"application/atom+xml\" href=\"EXPECTED\" /></head></html>");
            Assert.AreEqual("http://foo.bar/EXPECTED", actual);
        }

        [TestMethod]
        public void RetrieveFavironUri_Uri_FindUri()
        {
            string actual = SubscriptionFactory.RetrieveFaviconUri(
                "some-url", "<html><head><link rel=\"shortcut icon\" href=\"EXPECTED\" /></head></html>");
            Assert.AreEqual("EXPECTED", actual);
        }
    }
}
