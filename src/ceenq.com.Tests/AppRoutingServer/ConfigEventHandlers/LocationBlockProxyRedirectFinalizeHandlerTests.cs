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
    public class LocationBlockProxyRedirectFinalizeHandlerTests
    {
        [Test]
        public void ShouldSetProxyRedirectWhenProxyPassIsPopulated()
        {
            var accountContext = new Mock<IAccountContext>();
            var application = new Mock<IApplication>();
            var locationBlock = new LocationBlock();
            var route = new Mock<IRoute>();

            locationBlock.ProxyPass = "http://some.external.com/path/$1$is_args$args";
            var locationBlockContext = new LocationBlockContext(locationBlock, application.Object, route.Object, accountContext.Object);
            var handler = new LocationBlockProxyRedirectFinalizeHandler();
            handler.FinalizeLocationBlock(locationBlockContext);
            Assert.That(!string.IsNullOrWhiteSpace(locationBlock.ProxyRedirect), "This test expected the the ProxyRedirect property would be set");
        }

    }
}
