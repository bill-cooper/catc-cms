using System;
using ceenq.com.Core.Infrastructure.Compute;
using ceenq.com.Core.Routing;
using Orchard;
using Orchard.Logging;
using Orchard.UI.Notify;

namespace ceenq.com.RoutingServer
{
    public class RoutingServerDeletionEventHandler :Component, IRoutingServerDeletionEventHandler
    {
        private readonly IServerManagement _serverManagement;
        private readonly INotifier _notifier;

        public RoutingServerDeletionEventHandler(IServerManagement serverManagement, INotifier notifier)
        {
            _serverManagement = serverManagement;
            _notifier = notifier;
        }

        public void Deleting(RoutingServerDeletionEventContext context)
        {
        }

        public void Deleted(RoutingServerDeletionEventContext context)
        {
            //if no ip address is present on the record, then take no action
            // there must not be an assoicated vm for some reason.
            if (string.IsNullOrWhiteSpace(context.RoutingServer.IpAddress))
            {
                _notifier.Warning(T("No IP Address is associated with this routing server, so no action was taken to delete an associated VM."));
                return;
            }

            //delete the associated VM
            try
            {
                _serverManagement.Delete(new ServerOperationParameters()
                {
                    Name = context.RoutingServer.Name
                });
                _notifier.Information(T("Associated VM successfully deleted for routing server: {0} ({1})", context.RoutingServer.Name, context.RoutingServer.IpAddress));
            }
            catch (Exception ex)
            {
                _notifier.Error(T("Failed to delete the associated VM for routing server: {0} ({1})", context.RoutingServer.Name, context.RoutingServer.IpAddress));
                Logger.Error(ex, "Failed to delete VM for routing server: {0} ({1})", context.RoutingServer.Name, context.RoutingServer.IpAddress);
            }
        }
    }
}