using Orchard.ContentManagement;

namespace ceenq.com.Core.Routing
{
    public interface IApplicationRoutingServer : IContent
    {
        string IpAddress { get; set; }
    }
}