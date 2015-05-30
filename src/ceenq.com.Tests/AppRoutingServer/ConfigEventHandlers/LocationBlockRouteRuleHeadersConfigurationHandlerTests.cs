using ceenq.com.AppRoutingServer.ConfigEventHandlers;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Routing;
using ceenq.com.RoutingServer.Configuration;
using Moq;
using NUnit.Framework;

namespace ceenq.com.Tests.AppRoutingServer.ConfigEventHandlers
{
    [TestFixture]
    public class LocationBlockRouteRuleHeadersConfigurationHandlerTests
    {
        [Test]
        public void ShouldAddRoutRulesAsHeaderEntriesOnLocationBlock()
        {
            var accountContext = new Mock<IAccountContext>();
            var application = new Mock<IApplication>();
            var locationBlock = new LocationBlock();
            var route = new Mock<IRoute>();

            route.SetupGet(r => r.Rules).Returns("key1=value1, key2=value2");

            var locationBlockContext = new LocationBlockContext(locationBlock,application.Object, route.Object, accountContext.Object);
            var handler = new LocationBlockRouteRuleHeadersConfigurationHandler();
            handler.ConfigureLocationBlock(locationBlockContext);
            Assert.That(locationBlock.ProxySetHeader.Count == 2, "This test expected two entries to be populated in ProxySetHeader collection based on the configured route rules.");
            Assert.That(locationBlock.ProxySetHeader[0].StartsWith("key1"), "This test expected the first entry in ProxySetHeader to be based on the first route rule.");
        }
        [Test]
        [ExpectedException(typeof(ConfigGenerationException), ExpectedMessage = "Could not configure location block.  The config context was not supplied.")]
        public void ShouldThrowExceptionIfConfigContextIsMissing()
        {
            var handler = new LocationBlockRouteRuleHeadersConfigurationHandler();
            handler.ConfigureLocationBlock(null);
        }
    }
}
