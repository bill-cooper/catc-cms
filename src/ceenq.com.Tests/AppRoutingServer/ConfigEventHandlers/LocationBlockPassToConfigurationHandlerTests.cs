using ceenq.com.AppRoutingServer.ConfigEventHandlers;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Routing;
using ceenq.com.RoutingServer.Configuration;
using Moq;
using NUnit.Framework;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.FieldStorage.InfosetStorage;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Settings.Models;
using Orchard.Environment.Configuration;
using Orchard.Settings;

namespace ceenq.com.Tests.AppRoutingServer.ConfigEventHandlers
{
    [TestFixture]
    public class LocationBlockPassToConfigurationHandlerTests
    {
        private IAccountContext InitAccountContext()
        {
            var siteService = new Mock<ISiteService>();
            var site = new Mock<ISite>();
            var workContextAccessor = new Mock<IWorkContextAccessor>();
            var simulationType = new ContentTypeDefinitionBuilder().Named("Settings").Build();
            var contentItem = new ContentItemBuilder(simulationType)
                .Weld<SiteSettingsPart>()
                .Weld<InfosetPart>()
                .Build();
            contentItem.As<SiteSettingsPart>().BaseUrl = "http://www.somesite.com";


            site.SetupGet(s => s.ContentItem).Returns(contentItem);
            siteService.Setup(s => s.GetSiteSettings()).Returns(site.Object);
            var workContext = new Mock<WorkContext>();
            workContext.Setup(w => w.GetState<ISite>(It.Is<string>(s => s == "CurrentSite"))).Returns(() => siteService.Object.GetSiteSettings());

            workContextAccessor.Setup(w => w.GetContext()).Returns(workContext.Object);

            var accountContext = new DefaultAccountContext(new ShellSettings() { Name = "someaccount" }, siteService.Object, workContextAccessor.Object);

            return accountContext;
        }

        [Test]
        public void ShouldNotSetProxyPassIfAlreadyHasValue()
        {
            var accountContext = new Mock<IAccountContext>();
            var application = new Mock<IApplication>();
            var locationBlock = new LocationBlock();
            var route = new Mock<IRoute>();
            var routeService = new Mock<IRouteService>();

            locationBlock.ProxyPass = "/some-pass-path";

            var locationBlockContext = new LocationBlockContext(locationBlock, application.Object, route.Object, accountContext.Object);
            var handler = new LocationBlockPassToAdjustHandler(routeService.Object);
            handler.AdjustLocationBlock(locationBlockContext);
            Assert.That(locationBlock.ProxyPass == "/some-pass-path", "This test did not expected for ProxyPass to be set to some other value.");
            Assert.That(string.IsNullOrEmpty(locationBlock.Root), "This test did not expected for a value to be set for Root.");
        }
        [Test]
        public void ShouldNotSetRootIfAlreadyHasValue()
        {
            var accountContext = new Mock<IAccountContext>();
            var application = new Mock<IApplication>();
            var locationBlock = new LocationBlock();
            var route = new Mock<IRoute>();
            var routeService = new Mock<IRouteService>();

            locationBlock.Root = "/some-root-path";

            var locationBlockContext = new LocationBlockContext(locationBlock, application.Object, route.Object, accountContext.Object);
            var handler = new LocationBlockPassToAdjustHandler(routeService.Object);
            handler.AdjustLocationBlock(locationBlockContext);
            Assert.That(locationBlock.Root == "/some-root-path", "This test did not expected for Root to be set to some other value.");
            Assert.That(string.IsNullOrEmpty(locationBlock.ProxyPass), "This test did not expected for a value to be set for ProxyPass.");
        }
        [Test]
        public void ShouldSetRootToToServerAssetPathIfRoutesToLocalResourceAndDoesNotRequireAuth()
        {
            var locationBlock = new LocationBlock();
            var application = new Mock<IApplication>();
            var route = new Mock<IRoute>();
            var routeService = new Mock<IRouteService>();
            var accountContext = InitAccountContext();

            route.SetupGet(r => r.RequireAuthentication).Returns(false);
            route.SetupGet(r => r.RequestPattern).Returns("/");
            route.SetupGet(r => r.PassTo).Returns("/some-local-resource");

            var locationBlockContext = new LocationBlockContext(locationBlock, application.Object, route.Object, accountContext);
            var handler = new LocationBlockPassToAdjustHandler(routeService.Object);
            handler.AdjustLocationBlock(locationBlockContext);

            Assert.That(!string.IsNullOrEmpty(locationBlock.Root), "This test expected for the Root to be set to some value.");
            Assert.That(string.IsNullOrEmpty(locationBlock.ProxyPass), "This test did not expected for a value to be set for ProxyPass.");

        }
        [Test]
        public void ShouldSetPassPathToInternalAssetPathIfRoutesToLocalResourceAndDoesRequireAuth()
        {
            var locationBlock = new LocationBlock();
            var application = new Mock<IApplication>();
            var route = new Mock<IRoute>();
            var routeService = new Mock<IRouteService>();
            var accountContext = InitAccountContext();

            route.SetupGet(r => r.RequireAuthentication).Returns(true);
            route.SetupGet(r => r.RequestPattern).Returns("/");
            route.SetupGet(r => r.PassTo).Returns("/some-local-resource");

            var locationBlockContext = new LocationBlockContext(locationBlock, application.Object, route.Object, accountContext);
            var handler = new LocationBlockPassToAdjustHandler(routeService.Object);
            handler.AdjustLocationBlock(locationBlockContext);

            Assert.That(string.IsNullOrEmpty(locationBlock.Root), "This test did not expected for Root to be set.");
            Assert.That(!string.IsNullOrEmpty(locationBlock.ProxyPass), "This test expected for the ProxyPass to be set to some value.");
        }
        [Test]
        public void ShouldSetPassPathToAbsoluteModulePathIfRoutesToModuleResource()
        {
            var accountContext = InitAccountContext();
            var application = new Mock<IApplication>();
            var locationBlock = new LocationBlock();
            var route = new Mock<IRoute>();
            var routeService = new Mock<IRouteService>();

            route.SetupGet(r => r.RequireAuthentication).Returns(true);
            route.SetupGet(r => r.RequestPattern).Returns("/");
            route.SetupGet(r => r.PassTo).Returns("$cms");

            locationBlock.MatchPattern = "/";

            routeService.Setup(r => r.RoutesToModuleResource(It.Is<IRoute>((rr) => rr == route.Object))).Returns(true);

            var locationBlockContext = new LocationBlockContext(locationBlock, application.Object, route.Object, accountContext);
            var handler = new LocationBlockPassToAdjustHandler(routeService.Object);
            handler.AdjustLocationBlock(locationBlockContext);
            Assert.That(locationBlock.ProxyPass == accountContext.ToAbsoluteModulePath(route.Object.PassTo).TrimEnd(new[] { '/' }) + "$request_uri", "This test expected the ProxyPass to be set to the associated absolute module path");
        }
        [Test]
        public void ShouldSetPassPathToExternalResoureIfRoutesToExternalResource()
        {
            var accountContext = InitAccountContext();
            var application = new Mock<IApplication>();
            var locationBlock = new LocationBlock();
            var route = new Mock<IRoute>();
            var routeService = new Mock<IRouteService>();

            route.SetupGet(r => r.RequireAuthentication).Returns(true);
            route.SetupGet(r => r.RequestPattern).Returns("/");
            route.SetupGet(r => r.PassTo).Returns("http://somesite.com/something");

            var locationBlockContext = new LocationBlockContext(locationBlock, application.Object, route.Object, accountContext);
            var handler = new LocationBlockPassToAdjustHandler(routeService.Object);
            handler.AdjustLocationBlock(locationBlockContext);
            Assert.That(locationBlock.ProxyPass.StartsWith(route.Object.PassTo), "This test expected the ProxyPass to be set to the associated external resource");
        }
        [Test]
        public void ShouldNotAddARewriteForBasePath()
        {
            var accountContext = InitAccountContext();
            var application = new Mock<IApplication>();
            var locationBlock = new LocationBlock();
            var route = new Mock<IRoute>();
            var routeService = new Mock<IRouteService>();

            route.SetupGet(r => r.RequireAuthentication).Returns(true);
            route.SetupGet(r => r.RequestPattern).Returns("/");
            route.SetupGet(r => r.PassTo).Returns("/some-local-resource");

            var locationBlockContext = new LocationBlockContext(locationBlock, application.Object, route.Object, accountContext);
            var handler = new LocationBlockPassToAdjustHandler(routeService.Object);
            handler.AdjustLocationBlock(locationBlockContext);
            Assert.That(string.IsNullOrEmpty(locationBlock.Rewrite), "This test expected Rewrite not to be set.");

        }
        [Test]
        public void ShouldAddARewriteForLocalResourceOtherThanBasePath()
        {
            var accountContext = InitAccountContext();
            var application = new Mock<IApplication>();
            var locationBlock = new LocationBlock();
            var route = new Mock<IRoute>();
            var routeService = new Mock<IRouteService>();

            route.SetupGet(r => r.RequireAuthentication).Returns(false);
            route.SetupGet(r => r.RequestPattern).Returns("/something-other-than-base");
            route.SetupGet(r => r.PassTo).Returns("/some-local-resource");

            var locationBlockContext = new LocationBlockContext(locationBlock, application.Object, route.Object, accountContext);
            var handler = new LocationBlockPassToAdjustHandler(routeService.Object);
            handler.AdjustLocationBlock(locationBlockContext);
            Assert.That(!string.IsNullOrEmpty(locationBlock.Rewrite), "This test expected Rewrite value to be set.");
        }
        [Test]
        [ExpectedException(typeof(ConfigGenerationException), ExpectedMessage = "Could not configure location block.  The config context was not supplied.")]
        public void ShouldThrowExceptionIfConfigContextIsMissing()
        {
            var routeService = new Mock<IRouteService>();
            var handler = new LocationBlockPassToAdjustHandler(routeService.Object);
            handler.AdjustLocationBlock(null);
        }
    }
}
