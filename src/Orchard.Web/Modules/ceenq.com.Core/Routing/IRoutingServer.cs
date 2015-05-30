using Orchard.ContentManagement;

namespace ceenq.com.Core.Routing
{
    public interface IRoutingServer : IContent
    {
        int Id { get; }
        string Name { get; set; }
        string DnsName { get; set; }
        int ConnectionPort { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string IpAddress { get; set; }
    }
}