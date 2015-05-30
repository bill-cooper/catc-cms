using System.Linq;
using ceenq.com.AppRoutingServer.Models;
using ceenq.com.AppRoutingServer.ViewModels;
using ceenq.com.Core.Routing;
using ceenq.com.RoutingServer.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace ceenq.com.AppRoutingServer.Drivers
{
    public class ApplicationRoutingServerPartDriver : ContentPartDriver<ApplicationRoutingServerPart>
    {
        private readonly IRoutingServerManager _routingServerManager;
        public ApplicationRoutingServerPartDriver(IRoutingServerManager routingServerManager)
        {
            _routingServerManager = routingServerManager;
        }

        protected override string Prefix
        {
            get { return "ApplicationRoutingServerPart"; }
        }


        protected override DriverResult Editor(ApplicationRoutingServerPart part, dynamic shapeHelper)
        {
            var viewModel = new ApplicationRoutingServerViewModel()
            {
                RoutingServers = _routingServerManager.List().Select(r => r.IpAddress).ToList()
            };
            if (part.IpAddress != null)
                viewModel.SelectedRoutingServer = part.IpAddress;


            return ContentShape("Parts_Application_ApplicationRoutingServerPart",
                () =>
                    shapeHelper.EditorTemplate(TemplateName: "Parts.Application.ApplicationRoutingServerPart",
                        Model: viewModel, Prefix: Prefix));
        }

        protected override DriverResult Editor(ApplicationRoutingServerPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var viewModel = new ApplicationRoutingServerViewModel()
            {
                RoutingServers = _routingServerManager.List().Select(r => r.IpAddress).ToList()
            };

            updater.TryUpdateModel(viewModel, Prefix, null, null);

            part.IpAddress = viewModel.SelectedRoutingServer;

            return ContentShape("Parts_Application_ApplicationRoutingServerPart",
            () =>
                shapeHelper.EditorTemplate(TemplateName: "Parts.Application.ApplicationRoutingServerPart",
                    Model: viewModel, Prefix: Prefix));
        }

    }
}