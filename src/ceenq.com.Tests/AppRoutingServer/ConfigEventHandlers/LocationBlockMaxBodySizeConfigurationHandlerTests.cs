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
    public class LocationBlockMaxBodySizeConfigurationHandlerTests
    {
        [Test]
        public void ShouldAddMaxBodySize()
        {
            var accountContext = new Mock<IAccountContext>();
            var application = new Mock<IApplication>();
            var locationBlock = new LocationBlock();
            var route = new Mock<IRoute>();
            var locationBlockContext = new LocationBlockContext(locationBlock,application.Object, route.Object, accountContext.Object);
            var handler = new LocationBlockMaxBodySizeConfigurationHandler();
            handler.ConfigureLocationBlock(locationBlockContext);
            Assert.That(!string.IsNullOrWhiteSpace(locationBlock.ClientMaxBodySize), "This test expected the ClientMaxBodySize to be set.");
        }
        [Test]
        [ExpectedException(typeof(ConfigGenerationException), ExpectedMessage = "Could not configure location block.  The config context was not supplied.")]
        public void ShouldThrowExceptionIfConfigContextIsMissing()
        {
            var handler = new LocationBlockMaxBodySizeConfigurationHandler();
            handler.ConfigureLocationBlock(null);
        }
    }
}
