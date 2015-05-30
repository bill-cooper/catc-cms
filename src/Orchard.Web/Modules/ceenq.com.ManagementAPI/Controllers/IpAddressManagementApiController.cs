using System.Linq;
using System.Web.Http;
using ceenq.com.ManagementAPI.Models;
using ceenq.com.RoutingServer.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Logging;

namespace ceenq.com.ManagementAPI.Controllers
{
    public class IpAddressManagementApiController : ApiController
    {
        private readonly IOrchardServices _orchardServices;
        public IpAddressManagementApiController(
            IOrchardServices orchardServices
            )
        {
            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public Localizer T { get; set; }
        public ILogger Logger { get; set; }


        public IHttpActionResult Get()
        {
            var routingServers = _orchardServices.ContentManager.Query<RoutingServerPart, RoutingServerRecord>().List();
            var model = routingServers.Select(r => new Endpoint() { IpAddress = r.IpAddress, Name = r.IpAddress });
            return Ok(model);
        }
        public IHttpActionResult Get(string address)
        {
            var routingServer =
                _orchardServices.ContentManager.Query<RoutingServerPart, RoutingServerRecord>()
                    .Where(r => r.IpAddress == address)
                    .List()
                    .FirstOrDefault();

            if (routingServer == null) return BadRequest(T("Bad Request").Text);

            var model = new Endpoint() { IpAddress = routingServer.IpAddress, Name = routingServer.IpAddress};

            return Ok(model);
        }

        public IHttpActionResult Post()
        {
            var routingServer = _orchardServices.ContentManager.New<RoutingServerPart>("RoutingServer");
            _orchardServices.ContentManager.Create(routingServer, VersionOptions.Published);

            return Ok();
        }
        public IHttpActionResult Delete(string address)
        {
            var routingServer =
                _orchardServices.ContentManager.Query<RoutingServerPart, RoutingServerRecord>()
                    .Where(r => r.IpAddress == address)
                    .List()
                    .FirstOrDefault();
            if (routingServer == null) return BadRequest(T("Bad Request").Text);
            _orchardServices.ContentManager.Remove(routingServer.ContentItem);
            return Ok();
        }


    }
}
