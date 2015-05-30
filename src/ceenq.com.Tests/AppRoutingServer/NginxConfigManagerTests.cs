using System;
using ceenq.com.AppRoutingServer.Services;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Infrastructure.Compute;
using ceenq.com.Core.Routing;
using Moq;
using NUnit.Framework;
using Orchard;

namespace ceenq.com.Tests.AppRoutingServer
{
    [TestFixture]
    public class NginxConfigManagerTests
    {
        const string AppName = "the-app";
        const string AccountName = "the-account";
        const string RoutingServer = "the.routing.server";
        private readonly string _configFileName = string.Format("{0}.{1}.conf", AppName, AccountName);
        private IApplication _application;
        private IAccountContext _accountContext;
        private IRoutingServer _routingServer;
        private readonly Mock<IGetFileCommand> _getFileCommand = new Mock<IGetFileCommand>();
        private readonly Mock<IWriteFileCommand> _writeFileCommand = new Mock<IWriteFileCommand>();
        private readonly Mock<IDeleteCommand> _deleteCommand = new Mock<IDeleteCommand>();
        private readonly Mock<INginxRestartCommand> _nginxRestartCommand = new Mock<INginxRestartCommand>();
        private readonly Mock<INginxReloadCommand> _nginxReloadCommand = new Mock<INginxReloadCommand>();
        [SetUp]
        public void Init()
        {
            var applicationMock = new Mock<IApplication>();
            applicationMock.SetupGet(a => a.Name).Returns(AppName);
            _application = applicationMock.Object;

            var accountContextMock = new Mock<IAccountContext>();
            accountContextMock.SetupGet(a => a.Account).Returns(AccountName);
            _accountContext = accountContextMock.Object;

            var routingServerMock = new Mock<IRoutingServer>();
            routingServerMock.SetupGet(r => r.DnsName).Returns(RoutingServer);
            _routingServer = routingServerMock.Object;

        }

        [Test]
        [ExpectedException(typeof(OrchardFatalException), ExpectedMessage = "The routing server config could not be generated for application " + AppName + " on routing server " + RoutingServer)]
        public void ShouldThrowExceptionIfConfigGenerationFails()
        {
            var serverCommandProvider = new Mock<IServerCommandProvider>();
            var routingServerManager = new Mock<IRoutingServerManager>();
            var routingServerConfigService = new Mock<IRoutingServerConfigService>();
            routingServerConfigService.Setup(
                r => r.GenerateConfig(It.Is<IApplication>(a => a.Equals(_application))))
                .Throws(new Exception("config generation failed"));

            var nginxConfigManager = new NginxConfigManager(_accountContext, serverCommandProvider.Object,
                routingServerConfigService.Object, routingServerManager.Object);
            nginxConfigManager.SaveConfig(_application, _routingServer);
        }

        [Test]
        public void GetExistingFileLogsAnException()
        {
            var serverCommandProvider = new Mock<IServerCommandProvider>();

            serverCommandProvider.Setup(c => c.New<IWriteFileCommand>(It.IsAny<string[]>())).Returns(_writeFileCommand.Object);
            serverCommandProvider.Setup(c => c.New<IDeleteCommand>(It.IsAny<string[]>())).Returns(_deleteCommand.Object);
            serverCommandProvider.Setup(c => c.New<INginxRestartCommand>(It.IsAny<string[]>())).Returns(_nginxRestartCommand.Object);
            serverCommandProvider.Setup(c => c.New<INginxReloadCommand>(It.IsAny<string[]>())).Returns(_nginxReloadCommand.Object);
            serverCommandProvider.Setup(c => c.New<IGetFileCommand>(It.Is<string>(s => s == _configFileName))).Returns(_getFileCommand.Object);

            var serverCommandClient = new Mock<IServerCommandClient>();
            var routingServerManager = new Mock<IRoutingServerManager>();
            routingServerManager.Setup(r => r.GetCommandClient(It.Is<IRoutingServer>(rs => rs.Equals(_routingServer)))).Returns(serverCommandClient.Object);
            serverCommandClient.Setup(s => s.ExecuteCommand(It.IsAny<IServerCommand>()))
                .Returns(new ServerCommandResult(string.Empty));
            serverCommandClient.Setup(s => s.ExecuteCommand(It.Is<IServerCommand>(c => c.Equals(_getFileCommand.Object))))
                .Throws(new Exception("command failed"));

            var routingServerConfigService = new Mock<IRoutingServerConfigService>();
            routingServerConfigService.Setup(
                r => r.GenerateConfig(It.Is<IApplication>(a => a.Equals(_application))))
                .Returns("generated config text....");

            var logger = new FakeLogger();

            var nginxConfigManager = new NginxConfigManager(_accountContext, serverCommandProvider.Object, routingServerConfigService.Object, routingServerManager.Object)
            {
                Logger = logger
            };
            nginxConfigManager.SaveConfig(_application, _routingServer);
            Assert.That(logger, Has.Property("Message").EqualTo(string.Format("Failure trying to read existing routing server config file.  Could not read file {0} from routing server {1}.", _configFileName, _routingServer.DnsName)));
        }

        [Test]
        public void WriteNewConfigThrowsException()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void DeleteCacheThrowsException()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void NginxRestartThrowsException()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void RestoreThrowsException()
        {
            throw new NotImplementedException();
        }



        private class FakeServerCommand : IServerCommand
        {
            public void Initialize(params string[] parameters)
            {
            }

            public string CommandText { get; private set; }
        }
    }
}
