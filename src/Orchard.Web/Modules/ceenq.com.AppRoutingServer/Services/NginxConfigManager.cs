using System;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Infrastructure.Compute;
using ceenq.com.Core.Routing;
using Orchard;
using Orchard.Environment.Extensions;
using Orchard.Logging;

namespace ceenq.com.AppRoutingServer.Services
{
    [OrchardSuppressDependency("ceenq.com.Core.Routing.DefaultRoutingServerConfigManager")]
    public class NginxConfigManager : Component, IRoutingServerConfigManager
    {
        private readonly IRoutingServerConfigService _routingServerConfigService;
        private readonly IAccountContext _accountContext;
        private readonly IServerCommandProvider _serverCommandProvider;
        private readonly IRoutingServerManager _routingServerManager;
        public NginxConfigManager(
            IAccountContext accountContext,
            IServerCommandProvider serverCommandProvider,
            IRoutingServerConfigService routingServerConfigService,
            IRoutingServerManager routingServerManager)
        {
            _accountContext = accountContext;
            _serverCommandProvider = serverCommandProvider;
            _routingServerConfigService = routingServerConfigService;
            _routingServerManager = routingServerManager;
        }

        public void SaveConfig(IApplication application, IRoutingServer routingServer)
        {
            string configFile;
            try
            {
                configFile = _routingServerConfigService.GenerateConfig(application);
            }
            catch (Exception ex)
            {
                throw new OrchardFatalException(T("The routing server config could not be generated for application {0} on routing server {1}", application.Name, routingServer.DnsName), ex);
            }

            SaveConfig(application, routingServer, configFile);
        }

        public void SaveConfig(IApplication application, IRoutingServer routingServer, string configFile)
        {

            var fileName = string.Format("{0}.{1}.conf", application.Name, _accountContext.Account);
            var client = _routingServerManager.GetCommandClient(routingServer);

            var existingConfigFile = string.Empty;
            try
            {
                var result = client.ExecuteCommand(_serverCommandProvider.New<IGetFileCommand>(fileName));
                existingConfigFile = result.Message;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failure trying to read existing routing server config file.  Could not read file {0} from routing server {1}.", fileName, routingServer.DnsName);
            }

            bool currentBadConfiguration = false;
            try
            {
                //run a reload only to see if the current app config is bad
                client.ExecuteCommand(_serverCommandProvider.New<INginxReloadCommand>());
            }
            catch (Exception ex)
            {
                currentBadConfiguration = true;
                Logger.Warning(ex, "Detected bad application configuration on routing server prior to saving a config file.  Could not reload nginx.  Could not read file {0} from routing server {1}.", fileName, routingServer.DnsName);
            }

            try
            {
                client.ExecuteCommand(_serverCommandProvider.New<IWriteFileCommand>(fileName, configFile));
            }
            catch (Exception ex)
            {
                throw new OrchardFatalException(T("The routing server config could not be saved for application {0} on routing server {1}.  Config File {2}: \n {3}", application.Name, routingServer.DnsName, fileName, configFile), ex);
            }

            try
            {
                //clear cache
                client.ExecuteCommand(_serverCommandProvider.New<IDeleteCommand>("ngcache"));
                client.ExecuteCommand(_serverCommandProvider.New<INginxReloadCommand>());
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Error, ex, "Failure to restart nginx after updating config for application {0} on routing server {1}.  Config File {2}: \n {3}", application.Name, routingServer.DnsName, fileName, configFile);
                //attempt to roll back the config to the original state so that nginx will not be stuck with a bad config file
                //only do this if there was not alread a bad config on the server
                if (!currentBadConfiguration && !string.IsNullOrWhiteSpace(existingConfigFile))
                {
                    Logger.Information("Attempting to roll back config for application {0} on routing server {1}.",
                        application.Name, routingServer.DnsName);
                    try
                    {
                        client.ExecuteCommand(_serverCommandProvider.New<IWriteFileCommand>(fileName, existingConfigFile));
                        client.ExecuteCommand(_serverCommandProvider.New<INginxReloadCommand>());
                    }
                    catch (Exception ex2)
                    {
                        throw new OrchardFatalException(
                            T("Failure to roll back config file for application {0} on routing server {1}.",
                                application.Name, routingServer.DnsName), ex2);
                    }
                }
                else
                {
                    throw new OrchardFatalException(T("There is a problem with the config file for application {0} on routing server {1}.", application.Name, routingServer.DnsName));
                }

                throw new OrchardFatalException(T("Failure to restart nginx after updating config for application {0} on routing server {1}.  Original config has been rolled back.  {2}", application.Name, routingServer.DnsName, ex.Message), ex);
            }

        }

        public void DeleteConfig(IApplication application, IRoutingServer routingServer)
        {
            var fileName = string.Format("{0}.{1}.conf", application.Name, _accountContext.Account);
            var client = _routingServerManager.GetCommandClient(routingServer);

            try
            {
                client.ExecuteCommand(_serverCommandProvider.New<IDeleteCommand>(fileName));
                client.ExecuteCommand(_serverCommandProvider.New<INginxReloadCommand>());
            }
            catch (Exception ex)
            {
                throw new OrchardFatalException(T("Failed to delete config file for application {0} on routing server {1}.", application.Name, routingServer.DnsName), ex);
            }
        }
    }
}
