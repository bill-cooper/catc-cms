using System;
using ceenq.com.Core.Infrastructure.Compute;
using ceenq.com.Core.Routing;
using Orchard;
using Orchard.Logging;
using Orchard.UI.Admin.Notification;
using Orchard.UI.Notify;

namespace ceenq.com.RoutingServer
{
    public class RoutingServerMaintenanceEventHandler :Component, IRoutingServerMaintenanceEventHandler
    {
        private readonly IServerManagement _serverManagement;
        private readonly INotifier _notifier;

        public RoutingServerMaintenanceEventHandler(IServerManagement serverManagement, INotifier notifier)
        {
            _serverManagement = serverManagement;
            _notifier = notifier;
        }


        public void Restart(RoutingServerMaintenanceEventContext context)
        {
            try
            {
                _serverManagement.Reboot(new ServerOperationParameters()
                {
                    Name = context.RoutingServer.Name
                });
            }
            catch (Exception ex)
            {
                _notifier.Error(T("Failed to reboot the associated VM for routing server: {0} ({1})", context.RoutingServer.Name, context.RoutingServer.IpAddress));
                Logger.Error(ex, "Failed to reboot VM for routing server: {0} ({1})", context.RoutingServer.Name, context.RoutingServer.IpAddress);
            }
        }
        public void PowerOn(RoutingServerMaintenanceEventContext context)
        {
            try
            {
                _serverManagement.PowerOn(new ServerOperationParameters()
                {
                    Name = context.RoutingServer.Name
                });
            }
            catch (Exception ex)
            {
                _notifier.Error(T("Failed to power on the associated VM for routing server: {0} ({1})", context.RoutingServer.Name, context.RoutingServer.IpAddress));
                Logger.Error(ex, "Failed to power on VM for routing server: {0} ({1})", context.RoutingServer.Name, context.RoutingServer.IpAddress);
            }
        }
        public void PowerOff(RoutingServerMaintenanceEventContext context)
        {
            try
            {
                _serverManagement.PowerOff(new ServerOperationParameters()
                {
                    Name = context.RoutingServer.Name
                });
            }
            catch (Exception ex)
            {
                _notifier.Error(T("Failed to power off the associated VM for routing server: {0} ({1})", context.RoutingServer.Name, context.RoutingServer.IpAddress));
                Logger.Error(ex, "Failed to power off VM for routing server: {0} ({1})", context.RoutingServer.Name, context.RoutingServer.IpAddress);
            }
        }
    }
}