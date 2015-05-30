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
    public class LocationBlockHostHeaderConfigurationHandlerTests
    {
        [Test]
        public void ShouldAddHostHeaderIfRouteIsNotToLocalResource()
        {
            var accountContext = new Mock<IAccountContext>();
            var application = new Mock<IApplication>();
            var locationBlock = new LocationBlock();
            var route = new Mock<IRoute>();
            var routeService = new Mock<IRouteService>();
            route.SetupGet(r => r.PassTo).Returns("http://some-external-resource.com");
            var locationBlockContext = new LocationBlockContext(locationBlock,application.Object, route.Object, accountContext.Object);
            var handler = new LocationBlockHostHeaderConfigurationHandler(routeService.Object);
            handler.ConfigureLocationBlock(locationBlockContext);
            Assert.That(locationBlock.ProxySetHeader.Count > 0 && locationBlock.ProxySetHeader[0].StartsWith("App-Host"), "This test expected for the App-Host header to be set.");
            Assert.That(locationBlock.ProxyPassSetHeaders == "on", "This test expected for ProxyPassSetHeaders to be turned on.");
        }
        [Test]
        [ExpectedException(typeof(ConfigGenerationException), ExpectedMessage = "Could not configure location block.  The config context was not supplied.")]
        public void ShouldThrowExceptionIfConfigContextIsMissing()
        {
            var routeService = new Mock<IRouteService>();
            var handler = new LocationBlockHostHeaderConfigurationHandler(routeService.Object);
            handler.ConfigureLocationBlock(null);
        }
    }
}
