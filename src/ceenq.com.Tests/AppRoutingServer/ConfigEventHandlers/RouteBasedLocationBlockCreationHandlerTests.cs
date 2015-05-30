using System.Collections.Generic;
using ceenq.com.AppRoutingServer.ConfigEventHandlers;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Routing;
using ceenq.com.RoutingServer.Configuration;
using Moq;
using NUnit.Framework;
using Orchard.Autoroute.Services;

namespace ceenq.com.Tests.AppRoutingServer.ConfigEventHandlers
{
    [TestFixture]
    public class RouteBasedLocationBlockCreationHandlerTests
    {
        [Test]
        public void ShouldNotCreateLocationBlocksIfApplicationHasNoRoutes()
        {
            var locationBlockConfigurationHandler = new Mock<ILocationBlockConfigurationHandler>();
            var locationBlockAdjustHandler = new Mock<ILocationBlockAdjustHandler>();
            var locationBlockFinalizeHandler = new Mock<ILocationBlockFinalizeHandler>();
            var slugService = new Mock<ISlugService>();
            var application = new Mock<IApplication>();
            var accountContext = new Mock<IAccountContext>();
            var routeService = new Mock<IRouteService>();
            application.SetupGet(a => a.Routes).Returns(new IRoute[] { });
            var serverBlock = new ServerBlock();
            var configContext = new ServerBlockContext(serverBlock, application.Object, accountContext.Object);

            var handler = new RouteBasedLocationBlockCreationHandler(locationBlockConfigurationHandler.Object, locationBlockAdjustHandler.Object, locationBlockFinalizeHandler.Object, slugService.Object, routeService.Object);
            handler.AddLocationBlock(configContext);
            Assert.That(serverBlock.LocationBlocks.Count == 0, "This test expected that no location blocks would be created.");
        }
        [Test]
        public void ShouldCreateLocationBlocksIfApplicationHasRoutes()
        {
            var locationBlockConfigurationHandler = new Mock<ILocationBlockConfigurationHandler>();
            var locationBlockAdjustHandler = new Mock<ILocationBlockAdjustHandler>();
            var locationBlockFinalizeHandler = new Mock<ILocationBlockFinalizeHandler>();
            var slugService = new Mock<ISlugService>();
            var application = new Mock<IApplication>();
            var accountContext = new Mock<IAccountContext>();
            var routeService = new Mock<IRouteService>();
            application.SetupGet(a => a.Routes).Returns(new IRoute[] { 
                new FakeRoute
                {
                    RequestPattern = "/"
                } 
            });
            var serverBlock = new ServerBlock();

            var configContext = new ServerBlockContext(serverBlock, application.Object, accountContext.Object);

            var handler = new RouteBasedLocationBlockCreationHandler(locationBlockConfigurationHandler.Object, locationBlockAdjustHandler.Object, locationBlockFinalizeHandler.Object, slugService.Object, routeService.Object);
            handler.AddLocationBlock(configContext);
            Assert.That(serverBlock.LocationBlocks.Count == 1, "This test expected that one location block would be created.");
        }
        [Test]
        public void ShouldCreateOverloadedLocationBlocksIfApplicationHasOverloadedRoutes()
        {
            var slugEventHandler = new Mock<ISlugEventHandler>();
            var slugService = new DefaultSlugService(slugEventHandler.Object);
            var application = new Mock<IApplication>();
            var accountContext = new Mock<IAccountContext>();
            var routeService = new Mock<IRouteService>();
            application.SetupGet(a => a.Routes).Returns(new IRoute[] { 
                new FakeRoute
                {
                    RequestPattern = "/",
                    RouteOrder = 1
                } ,
                new FakeRoute
                {
                    RequestPattern = "/",
                    RouteOrder = 2
                } 
            });
            var generatedLocationBlockContexts = new List<LocationBlockContext>();
            var locationBlockConfigurationHandler = new Mock<ILocationBlockConfigurationHandler>();
            locationBlockConfigurationHandler.Setup(l => l.ConfigureLocationBlock(It.IsAny<LocationBlockContext>()))
                .Callback<LocationBlockContext>(generatedLocationBlockContexts.Add);
            var locationBlockAdjustHandler = new Mock<ILocationBlockAdjustHandler>();
            var locationBlockFinalizeHandler = new Mock<ILocationBlockFinalizeHandler>();

            var serverBlock = new ServerBlock();

            var configContext = new ServerBlockContext(serverBlock, application.Object, accountContext.Object);

            var handler = new RouteBasedLocationBlockCreationHandler(locationBlockConfigurationHandler.Object, locationBlockAdjustHandler.Object, locationBlockFinalizeHandler.Object, slugService, routeService.Object);
            handler.AddLocationBlock(configContext);

            Assert.That(generatedLocationBlockContexts.Count == 2, "This test expected two location block contexts to be created");
            Assert.That(generatedLocationBlockContexts[0].Route.RouteOrder == 1 && generatedLocationBlockContexts[0].LocationBlock.ErrorPage.Count > 0
                , "This test expected the first route to be at index 0 and for errors to have been set up.");
            Assert.That(generatedLocationBlockContexts[0].LocationBlock.ErrorPage[0].Contains(generatedLocationBlockContexts[1].LocationBlock.MatchPattern)
               , "This test expected the first location block would have forwarding to the second location block.");
        }

        [Test]
        [ExpectedException(typeof(ConfigGenerationException), ExpectedMessage = "Could not generate location block.  The config context was not supplied.")]
        public void ShouldThrowExceptionIfConfigContextIsMissing()
        {
            var locationBlockConfigurationHandler = new Mock<ILocationBlockConfigurationHandler>();
            var locationBlockAdjustHandler = new Mock<ILocationBlockAdjustHandler>();
            var locationBlockFinalizeHandler = new Mock<ILocationBlockFinalizeHandler>();
            var slugService = new Mock<ISlugService>();
            var routeService = new Mock<IRouteService>();
            var handler = new RouteBasedLocationBlockCreationHandler(locationBlockConfigurationHandler.Object, locationBlockAdjustHandler.Object, locationBlockFinalizeHandler.Object, slugService.Object, routeService.Object);
            handler.AddLocationBlock(null);
        }

        class FakeRoute : IRoute
        {
            public int Id { get; private set; }
            public string RequestPattern { get; set; }
            public string PassTo { get; set; }
            public int RouteOrder { get; set; }
            public bool RequireAuthentication { get; set; }
            public bool CachingEnabled { get; set; }
            public string Rules { get; set; }
        }
    }

}
