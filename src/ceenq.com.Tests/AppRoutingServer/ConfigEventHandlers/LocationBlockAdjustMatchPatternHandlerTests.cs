using System;
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
    public class LocationBlockAdjustMatchPatternHandlerTests
    {
        [Test]
        public void ShouldAddCaseInsensitiveRegularExpressionModifierForDirectoryRequestPattern()
        {
            var accountContext = new Mock<IAccountContext>();
            var application = new Mock<IApplication>();
            var locationBlock = new LocationBlock();
            var route = new Mock<IRoute>();

            locationBlock.MatchPattern = "/something";

            var locationBlockContext = new LocationBlockContext(locationBlock,application.Object, route.Object, accountContext.Object);
            var handler = new LocationBlockAdjustMatchPatternHandler();
            handler.AdjustLocationBlock(locationBlockContext);
            Assert.That(locationBlock.MatchPattern == "~* /something", "This test expected for the case-insensitive modifier to be added to the MatchPattern.");
        }
        [Test]
        public void ShouldNotModifyNamedLocationRequestPatterns()
        {
            var accountContext = new Mock<IAccountContext>();
            var application = new Mock<IApplication>();
            var locationBlock = new LocationBlock();
            var route = new Mock<IRoute>();

            locationBlock.MatchPattern = "@somelocation";

            var locationBlockContext = new LocationBlockContext(locationBlock,application.Object, route.Object, accountContext.Object);
            var handler = new LocationBlockAdjustMatchPatternHandler();
            handler.AdjustLocationBlock(locationBlockContext);
            Assert.That(locationBlock.MatchPattern == "@somelocation", "This test did not expect MatchPattern to be modified since this is referring to a named location.");
        }

        [Test]
        [ExpectedException(typeof(ConfigGenerationException), ExpectedMessage = "Could not adjust location block.  The config context was not supplied.")]
        public void ShouldThrowExceptionIfConfigContextIsMissing()
        {
            var handler = new LocationBlockAdjustMatchPatternHandler();
            handler.AdjustLocationBlock(null);
        }
    }
}
