using System;
using ceenq.com.Core.Infrastructure.Compute;
using Orchard;
using Orchard.Environment.Extensions;
using Renci.SshNet;


namespace ceenq.com.SshClient.Services
{
    [OrchardSuppressDependency("ceenq.com.Core.Infrastructure.Compute.DefaultServerCommandClient")]
    public class SshCommandClient : Component, IServerCommandClient, IDisposable
    {
        private readonly IServerCommandEventHandler _serverCommandEventHandler;
        private readonly Lazy<Renci.SshNet.SshClient> _sshClient;
        public SshCommandClient(SshConnectionInfo connectionInfo, IServerCommandEventHandler serverCommandEventHandler)
        {
            _serverCommandEventHandler = serverCommandEventHandler;

            _sshClient = new Lazy<Renci.SshNet.SshClient>(() =>
            {
                var auth = new PasswordAuthenticationMethod(connectionInfo.Username, connectionInfo.Password);

                var sshConnectionInfo = new Renci.SshNet.ConnectionInfo(
                    connectionInfo.Host,
                    connectionInfo.Port,
                    connectionInfo.Username,
                    auth);

                var client = new Renci.SshNet.SshClient(sshConnectionInfo);

                var timeout = new TimeSpan(0, 0, 0, connectionInfo.ConnectionTimeout);
                var stopTrying = false;
                var failuresOccured = false;
                var time = DateTime.Now;
                while (!stopTrying)
                {
                    try
                    {
                        client.Connect();
                        stopTrying = true;
                    }
                    catch (Exception ex)
                    {
                        failuresOccured = true;
                        if (DateTime.Now.Subtract(time).Milliseconds > timeout.TotalMilliseconds)
                        {
                            stopTrying = true;
                            throw new OrchardException(T("Failed to connect to the Routing Server.  Check connection settings and ensure that the server is online."), ex);
                        }
                        System.Threading.Thread.Sleep(500);
                    }
                }

                //since a connection failure occured, this could mean
                // that the vm has just recently come online.  Lets
                // pause for a few seconds to ensure that the vm
                // is ready to accept commands.
                if (failuresOccured)
                    System.Threading.Thread.Sleep(10000);//sleep 10 seconds

                return client;
            });
        }

        public ServerCommandResult ExecuteCommand(IServerCommand command)
        {
            var client = _sshClient.Value;
            var commandText = command.CommandText;
            var cmd = client.CreateCommand(commandText);
            cmd.Execute();
            if (cmd.ExitStatus > 0)
            {
                throw new ServerCommandException(T("Routing Server command failed with the following error: ExitStatus={1} {2}\n Result={3}\n For command:\n{0}\n", commandText, cmd.ExitStatus, cmd.Error, cmd.Result), commandText, cmd.ExitStatus, cmd.Error, cmd.Result);
            }
            _serverCommandEventHandler.CommandExecuted(new ServerCommandContext { Command = command, Message = cmd.Result });
            return new ServerCommandResult(cmd.Result);
        }

        public void Dispose()
        {
            if (!_sshClient.IsValueCreated || _sshClient.Value == null) return;

            if (_sshClient.Value.IsConnected)
                _sshClient.Value.Disconnect();
            _sshClient.Value.Dispose();
        }
    }
}