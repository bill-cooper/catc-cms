using ceenq.com.AppRoutingServer.ConfigEventHandlers;
using ceenq.com.Core.Routing;
using ceenq.com.RoutingServer.Configuration;
using Moq;
using NUnit.Framework;

namespace ceenq.com.Tests.AppRoutingServer.ConfigEventHandlers
{
    [TestFixture]
    public class LocationBlockResolverConfigurationHandlerTests
    {

        [Test]
        [ExpectedException(typeof(ConfigGenerationException), ExpectedMessage = "Could not adjust location block.  The config context was not supplied.")]
        public void ShouldThrowExceptionIfConfigContextIsMissing()
        {
            var routeService = new Mock<IRouteService>();
            var handler = new LocationBlockResolverConfigurationHandler(routeService.Object);
            handler.AdjustLocationBlock(null);
        }
    }
}
