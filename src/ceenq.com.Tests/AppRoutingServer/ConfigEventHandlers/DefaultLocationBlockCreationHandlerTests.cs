﻿using ceenq.com.AppRoutingServer.ConfigEventHandlers;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Routing;
using ceenq.com.RoutingServer.Configuration;
using Moq;
using NUnit.Framework;
using Orchard;
using Orchard.Environment.Configuration;
using Orchard.Settings;

namespace ceenq.com.Tests.AppRoutingServer.ConfigEventHandlers
{
    [TestFixture]
    public class DefaultLocationBlockCreationHandlerTests
    {
        [Test]
        public void ShouldNotAddLocationBlockIfABaseRouteHasBeenManuallyCreated()
        {
            var locationBlockConfigurationHandler = new Mock<ILocationBlockConfigurationHandler>();
            var locationBlockAdjustHandler = new Mock<ILocationBlockAdjustHandler>();
            var locationBlockFinalizeHandler = new Mock<ILocationBlockFinalizeHandler>();
            var serverBlock = new ServerBlock();
            var application = new Mock<IApplication>();
            var siteService = new Mock<ISiteService>();
            var workContextAccessor = new Mock<IWorkContextAccessor>();

            var accountContext = new DefaultAccountContext(new ShellSettings() { Name = "someaccount" }, siteService.Object, workContextAccessor.Object);
            var route = new Mock<IRoute>();
            route.SetupGet(r => r.RequestPattern).Returns("/");
            application.SetupGet(a => a.Routes).Returns(new[] {route.Object});

            var configContext = new ServerBlockContext(serverBlock, application.Object, accountContext);

            var handler = new DefaultLocationBlockCreationHandler(locationBlockConfigurationHandler.Object, locationBlockAdjustHandler.Object, locationBlockFinalizeHandler.Object);
            handler.AddLocationBlock(configContext);
            Assert.That(serverBlock.LocationBlocks.Count == 0, "This test did not expect any routes to be created for the location block, but some were created.");
       
        }
        [Test]
        public void ShouldAddLocationBlockIfABaseRouteHasNotBeenManuallyCreated()
        {
            var locationBlockConfigurationHandler = new Mock<ILocationBlockConfigurationHandler>();
            var locationBlockAdjustHandler = new Mock<ILocationBlockAdjustHandler>();
            var locationBlockFinalizeHandler = new Mock<ILocationBlockFinalizeHandler>();
            var serverBlock = new ServerBlock();
            var application = new Mock<IApplication>();
            var siteService = new Mock<ISiteService>();
            var workContextAccessor = new Mock<IWorkContextAccessor>();

            var accountContext = new DefaultAccountContext(new ShellSettings() { Name = "someaccount" }, siteService.Object, workContextAccessor.Object);

            var configContext = new ServerBlockContext(serverBlock, application.Object, accountContext);

            var handler = new DefaultLocationBlockCreationHandler(locationBlockConfigurationHandler.Object, locationBlockAdjustHandler.Object, locationBlockFinalizeHandler.Object);
            handler.AddLocationBlock(configContext);
            Assert.That(serverBlock.LocationBlocks.Count == 1, "This test expected that one location block would have been added to the server block, but this is not the case.");
        }
        [Test]
        [ExpectedException(typeof(ConfigGenerationException), ExpectedMessage = "Could not generate location block.  The config context was not supplied.")]
        public void ShouldThrowExceptionIfConfigContextIsMissing()
        {
            var locationBlockConfigurationHandler = new Mock<ILocationBlockConfigurationHandler>();
            var locationBlockAdjustHandler = new Mock<ILocationBlockAdjustHandler>();
            var locationBlockFinalizeHandler = new Mock<ILocationBlockFinalizeHandler>();
            var handler = new DefaultLocationBlockCreationHandler(locationBlockConfigurationHandler.Object, locationBlockAdjustHandler.Object, locationBlockFinalizeHandler.Object);
            handler.AddLocationBlock(null);
        }

    }
}
