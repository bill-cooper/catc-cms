using System;
using System.Collections.Generic;
using System.Linq;
using ceenq.com.Core.Infrastructure.Compute;
using ceenq.com.Core.Routing;
using ceenq.com.RoutingServer.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace ceenq.com.RoutingServer.Services
{
     [OrchardSuppressDependency("ceenq.com.Core.Routing.DefaultRoutingServerManager")]
    public class RoutingServerManager : Component, IRoutingServerManager, IRoutingServerProvider
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IServerCommandClientProvider _serverCommandClientProvider;
        private readonly IRoutingServerCreationEventHandler _routingServerCreationEventHandler;
        private readonly IRoutingServerDeletionEventHandler _routingServerDeletionEventHandler;
        private readonly IRoutingServerMaintenanceEventHandler _routingServerMaintenanceEventHandler;

        public RoutingServerManager(
            IOrchardServices orchardServices, 
            IServerCommandClientProvider serverCommandClientProvider, 
            IRoutingServerCreationEventHandler routingServerCreationEventHandler, 
            IRoutingServerDeletionEventHandler routingServerDeletionEventHandler, 
            IRoutingServerMaintenanceEventHandler routingServerMaintenanceEventHandler)
        {
            _orchardServices = orchardServices;
            _serverCommandClientProvider = serverCommandClientProvider;
            _routingServerCreationEventHandler = routingServerCreationEventHandler;
            _routingServerDeletionEventHandler = routingServerDeletionEventHandler;
            _routingServerMaintenanceEventHandler = routingServerMaintenanceEventHandler;
        }

        public IRoutingServer GetDefault()
        {
            return List().OrderBy(rs => rs.Id).FirstOrDefault();
        }

        public IServerCommandClient GetCommandClient(string ipAddress)
        {
            var routingServer = Get(ipAddress);
            return GetCommandClient(routingServer);
        }
        public IServerCommandClient GetCommandClient(IRoutingServer routingServer)
        {
            return _serverCommandClientProvider.Connect(new SshConnectionInfo()
            {
                Host = routingServer.DnsName,
                Port = routingServer.ConnectionPort,
                Username = routingServer.Username,
                Password = routingServer.Password
            });
        }

        public IServerCommandClient GetDefaultCommandClient()
        {
            var defaultRoutingServer = GetDefault();
            return GetCommandClient(defaultRoutingServer);
        }

        public IRoutingServer Get(string ipAddress)
        {
            if (String.IsNullOrWhiteSpace(ipAddress))
            {
                return null;
            }

            return _orchardServices.ContentManager.Query<RoutingServerPart, RoutingServerRecord>()
                .Where(x => x.IpAddress == ipAddress)
                .ForType("RoutingServer")
                .Slice(0, 1)
                .FirstOrDefault();
        }

        public void Delete(int id)
        {
            var routingServer = List().FirstOrDefault(r => r.Id == id);

            if (routingServer != null)
            {
                var context = new RoutingServerDeletionEventContext() {RoutingServer = routingServer};
                _routingServerDeletionEventHandler.Deleting(context);
                _orchardServices.ContentManager.Remove(routingServer.ContentItem);
                _routingServerDeletionEventHandler.Deleted(context);
            }
        }

        public void Restart(int id)
        {
            var routingServer = List().FirstOrDefault(r => r.Id == id);

            if (routingServer != null)
            {
                var context = new RoutingServerMaintenanceEventContext() { RoutingServer = routingServer };
                _routingServerMaintenanceEventHandler.Restart(context);
            }
        }
        public void PowerOn(int id)
        {
            var routingServer = List().FirstOrDefault(r => r.Id == id);

            if (routingServer != null)
            {
                var context = new RoutingServerMaintenanceEventContext() { RoutingServer = routingServer };
                _routingServerMaintenanceEventHandler.PowerOn(context);
            }
        }

        public void PowerOff(int id)
        {
            var routingServer = List().FirstOrDefault(r => r.Id == id);

            if (routingServer != null)
            {
                var context = new RoutingServerMaintenanceEventContext() { RoutingServer = routingServer };
                _routingServerMaintenanceEventHandler.PowerOff(context);
            }
        }

        public IEnumerable<IRoutingServer> List()
        {
            return _orchardServices.ContentManager.Query<RoutingServerPart, RoutingServerRecord>().List();
        }

        //public void Store(IRoutingServer routingServerRecord)
        //{
        //    _orchardServices.ContentManager.Publish(routingServerRecord.ContentItem);
        //}

        public IRoutingServer New()
        {
            var routingServer = _orchardServices.ContentManager.New<RoutingServerPart>("RoutingServer");
            var context = new RoutingServerCreationEventContext() { RoutingServer = routingServer };
            _routingServerCreationEventHandler.Creating(context);
            _orchardServices.ContentManager.Create(routingServer, VersionOptions.Published);
            _routingServerCreationEventHandler.Created(context);
            _routingServerCreationEventHandler.PostCreated(context);
            return routingServer;
        }

        public bool IsDefault(IRoutingServer routingServer)
        {
            var defaultRoutingServer = List().OrderBy(rs => rs.Id).FirstOrDefault();
            if (defaultRoutingServer != null && defaultRoutingServer.Id == routingServer.Id)
                return true;
            return false;
        }


    }
}