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
    public class LocationBlockAppNameConfigurationHandlerTests
    {
        [Test]
        public void ShouldNotAddAppNameWhenPassToExternalResource()
        {
            var accountContext = new Mock<IAccountContext>();
            var application = new Mock<IApplication>();
            var locationBlock = new LocationBlock();
            var route = new Mock<IRoute>(); 
            var routeService = new Mock<IRouteService>();
            route.SetupGet(r => r.PassTo).Returns("http://some-external-resource.com");
            var locationBlockContext = new LocationBlockContext(locationBlock, application.Object, route.Object, accountContext.Object);
            var handler = new LocationBlockAppNameConfigurationHandler(routeService.Object);
            handler.ConfigureLocationBlock(locationBlockContext);
            Assert.That(locationBlock.ProxySetHeader.Count == 0, "This test did not expect any ProxySetHeader values to be set");
        }
        [Test]
        public void ShouldAddAppNameWhenPassToInternalResource()
        {
            var accountContext = new Mock<IAccountContext>();
            var application = new Mock<IApplication>();
            var locationBlock = new LocationBlock();
            var route = new Mock<IRoute>();
            var routeService = new Mock<IRouteService>();
            route.SetupGet(r => r.PassTo).Returns("$cms");
            var locationBlockContext = new LocationBlockContext(locationBlock, application.Object, route.Object, accountContext.Object);
            var handler = new LocationBlockAppNameConfigurationHandler(routeService.Object);
            handler.ConfigureLocationBlock(locationBlockContext);
            Assert.That(locationBlock.ProxySetHeader.Count == 1, "This test expected a value to be added to ProxySetHeader");
            Assert.That(locationBlock.ProxySetHeader[0].StartsWith("App-Name"), "This test expected App-Name to be added to ProxySetHeader");
        }
        [Test]
        [ExpectedException(typeof(ConfigGenerationException), ExpectedMessage = "Could not configure location block.  The config context was not supplied.")]
        public void ShouldThrowExceptionIfConfigContextIsMissing()
        {
            var routeService = new Mock<IRouteService>();
            var handler = new LocationBlockAppNameConfigurationHandler(routeService.Object);
            handler.ConfigureLocationBlock(null);
        }
    }
}
