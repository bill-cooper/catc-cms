using System.Collections.Generic;
using ceenq.com.AppRoutingServer.ConfigEventHandlers;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Applications;
using ceenq.com.RoutingServer.Configuration;
using Moq;
using NUnit.Framework;

namespace ceenq.com.Tests.AppRoutingServer.ConfigEventHandlers
{
    [TestFixture]
    public class PrimaryServerBlockCreationHandlerTests
    {
        [Test]
        public void ShouldCreateAPrimaryServerBlock()
        {
            var application = new Mock<IApplication>();
            var accountContext = new Mock<IAccountContext>();
            var serverBlockConfigurationHandler = new Mock<IServerBlockConfigurationHandler>();
            var locationBlockCreationHandler = new Mock<ILocationBlockCreationHandler>();
            var configContext = new ConfigContext(new Config(), application.Object);
            var applicationDnsNamesService = new Mock<IApplicationDnsNamesService>();
            applicationDnsNamesService.Setup(s => s.DnsNames(It.IsAny<IApplication>()))
                .Returns(new List<string>() { "dns.name" });

            var handler = new PrimaryServerBlockCreationHandler(applicationDnsNamesService.Object, accountContext.Object, serverBlockConfigurationHandler.Object, locationBlockCreationHandler.Object);
            handler.AddServerBlock(configContext);

            Assert.That(configContext.Config.ServerBlock.Count == 1, "This test expected that 1 server blocks would have been created.");
        }

        [Test]
        [ExpectedException(typeof(ConfigGenerationException), ExpectedMessage = "Could not generate primary server block.  The config context was not supplied.")]
        public void ShouldThrowExceptionIfConfigContextIsMissing()
        {
            var accountContext = new Mock<IAccountContext>();
            var serverBlockConfigurationHandler = new Mock<IServerBlockConfigurationHandler>();
            var locationBlockCreationHandler = new Mock<ILocationBlockCreationHandler>();
            var applicationDnsNamesService = new Mock<IApplicationDnsNamesService>();
            var handler = new PrimaryServerBlockCreationHandler(applicationDnsNamesService.Object, accountContext.Object, serverBlockConfigurationHandler.Object, locationBlockCreationHandler.Object);
            handler.AddServerBlock(null);
        }

        [Test]
        [ExpectedException(typeof(ConfigGenerationException), ExpectedMessage = "Could not generate primary server block.  No DNS names are associated with application named 'app'")]
        public void ShouldThrowExceptionIfApplicationHasNoDnsNames()
        {
            var application = new Mock<IApplication>();
            var accountContext = new Mock<IAccountContext>();
            var serverBlockConfigurationHandler = new Mock<IServerBlockConfigurationHandler>();
            var locationBlockCreationHandler = new Mock<ILocationBlockCreationHandler>();
            application.SetupGet(a => a.Name).Returns("app");
            var configContext = new ConfigContext(new Config(), application.Object);
            var applicationDnsNamesService = new Mock<IApplicationDnsNamesService>();
            applicationDnsNamesService.Setup(s => s.DnsNames(It.IsAny<IApplication>()))
                .Returns(new List<string>()); //no dns names added.  This should throw an exception

            var handler = new PrimaryServerBlockCreationHandler(applicationDnsNamesService.Object, accountContext.Object, serverBlockConfigurationHandler.Object, locationBlockCreationHandler.Object);
            handler.AddServerBlock(configContext);
        }

        [Test]
        public void ShouldCreatePrimaryServerBlockWithSameDnsNamesAsApp()
        {
            var application = new Mock<IApplication>();
            var accountContext = new Mock<IAccountContext>();
            var serverBlockConfigurationHandler = new Mock<IServerBlockConfigurationHandler>();
            var locationBlockCreationHandler = new Mock<ILocationBlockCreationHandler>();
            application.SetupGet(a => a.Name).Returns("app");
            var configContext = new ConfigContext(new Config(), application.Object);
            var applicationDnsNamesService = new Mock<IApplicationDnsNamesService>();
            var dnsNames = new List<string>
            {
                "dns.name1",
                "dns.name2",
                "dns.name3"
            };
            applicationDnsNamesService.Setup(s => s.DnsNames(It.IsAny<IApplication>()))
                .Returns(dnsNames);

            var handler = new PrimaryServerBlockCreationHandler(applicationDnsNamesService.Object, accountContext.Object, serverBlockConfigurationHandler.Object, locationBlockCreationHandler.Object);
            handler.AddServerBlock(configContext);
            foreach (var dnsName in dnsNames)
            {
                Assert.Contains(dnsName, configContext.Config.ServerBlock[0].DnsNames);
            }
            //should have the same number of dnsNames as the application
            Assert.AreEqual(dnsNames.Count, configContext.Config.ServerBlock[0].DnsNames.Count);
        }

        [Test]
        public void ShouldCreatePrimaryServerBlockWithNoReturnValue()
        {
            var application = new Mock<IApplication>();
            var accountContext = new Mock<IAccountContext>();
            var serverBlockConfigurationHandler = new Mock<IServerBlockConfigurationHandler>();
            var locationBlockCreationHandler = new Mock<ILocationBlockCreationHandler>();
            var configContext = new ConfigContext(new Config(), application.Object);
            var applicationDnsNamesService = new Mock<IApplicationDnsNamesService>();
            applicationDnsNamesService.Setup(s => s.DnsNames(It.IsAny<IApplication>()))
                .Returns(new List<string>() { "dns.name" });

            var handler = new PrimaryServerBlockCreationHandler(applicationDnsNamesService.Object, accountContext.Object, serverBlockConfigurationHandler.Object, locationBlockCreationHandler.Object);
            handler.AddServerBlock(configContext);

            Assert.That(string.IsNullOrEmpty(configContext.Config.ServerBlock[0].Return), "The return value should never be set for the primar server block.");
        }

        [Test]
        public void ShouldCallAddLocationBlocksExactlyOnce()
        {
            var application = new Mock<IApplication>();
            var accountContext = new Mock<IAccountContext>();
            var serverBlockConfigurationHandler = new Mock<IServerBlockConfigurationHandler>();
            var locationBlockCreationHandler = new Mock<ILocationBlockCreationHandler>();
            var configContext = new ConfigContext(new Config(), application.Object);
            var applicationDnsNamesService = new Mock<IApplicationDnsNamesService>();
            applicationDnsNamesService.Setup(s => s.DnsNames(It.IsAny<IApplication>()))
                .Returns(new List<string>() { "dns.name" });

            var handler = new PrimaryServerBlockCreationHandler(applicationDnsNamesService.Object, accountContext.Object, serverBlockConfigurationHandler.Object, locationBlockCreationHandler.Object);
            handler.AddServerBlock(configContext);

            locationBlockCreationHandler.Verify(l => l.AddLocationBlock(It.IsAny<ServerBlockContext>()), Times.Once());
        }

    }

}
