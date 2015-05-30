using ceenq.com.AppRoutingServer.ConfigEventHandlers;
using ceenq.com.Core.Applications;
using ceenq.com.RoutingServer.Configuration;
using Moq;
using NUnit.Framework;

namespace ceenq.com.Tests.AppRoutingServer.ConfigEventHandlers
{
    [TestFixture]
    public class RedirectToNonWwwServerBlockCreationHandlerTests
    {
        [Test]
        public void ShouldCreateServerBlockIfAppHasCustomDomain()
        {
            var application = new Mock<IApplication>();
            application.SetupGet(a => a.Domain).Returns("customdomain.com");
            var configContext = new ConfigContext(new Config(), application.Object);

            var handler = new RedirectToNonWwwServerBlockCreationHandler();
            handler.AddServerBlock(configContext);

            Assert.That(configContext.Config.ServerBlock.Count == 1, "This test expected that 1 server blocks would have been created.");
        }
        [Test]
        public void ShouldCreateReturnValueIfAppHasCustomDomain()
        {
            var application = new Mock<IApplication>();
            application.SetupGet(a => a.Domain).Returns("customdomain.com");
            var configContext = new ConfigContext(new Config(), application.Object);

            var handler = new RedirectToNonWwwServerBlockCreationHandler();
            handler.AddServerBlock(configContext);

            Assert.That(configContext.Config.ServerBlock.Count == 1, "This test expected that 1 server blocks would have been created.");
            Assert.That(!string.IsNullOrEmpty(configContext.Config.ServerBlock[0].Return), "A return value is expected for the server block that has been generate.");
        }
        [Test]
        public void ShouldNotCreateServerBlockIfAppDoesNotHaveCustomDomain()
        {
            //no custom domain setup
            var application = new Mock<IApplication>();
            var configContext = new ConfigContext(new Config(), application.Object);

            var handler = new RedirectToNonWwwServerBlockCreationHandler();
            handler.AddServerBlock(configContext);

            Assert.That(configContext.Config.ServerBlock.Count == 0, "This test expected that no server blocks would have been created, but at least one was created.");
        }
        [Test]
        public void ShouldNotCreateServerBlockIfAppUsesTransportSecurity()
        {
            //no custom domain setup
            var application = new Mock<IApplication>();
            var configContext = new ConfigContext(new Config(), application.Object);
            application.SetupGet(a => a.TransportSecurity).Returns(true);

            var handler = new RedirectToNonWwwServerBlockCreationHandler();
            handler.AddServerBlock(configContext);

            Assert.That(configContext.Config.ServerBlock.Count == 0, "This test expected that no server blocks would have been created, but at least one was created.");
        }

        [Test]
        public void ShouldNotCreateServerBlockIfAppHasWwwCustomDomainSetup()
        {
            //no custom domain setup
            var application = new Mock<IApplication>();
            application.SetupGet(a => a.Domain).Returns("www.customdomain.com");
            var configContext = new ConfigContext(new Config(), application.Object);

            var handler = new RedirectToNonWwwServerBlockCreationHandler();
            handler.AddServerBlock(configContext);

            Assert.That(configContext.Config.ServerBlock.Count == 0, "This test expected that no server blocks would have been created, but at least one was created.");
        }

        [Test]
        [ExpectedException(typeof(ConfigGenerationException), ExpectedMessage = "Could not generate 'redirect to non-www' server block.  The config context was not supplied.")]
        public void ShouldThrowExceptionIfConfigContextIsMissing()
        {
            var handler = new RedirectToNonWwwServerBlockCreationHandler();
            handler.AddServerBlock(null);
        }

    }
}
