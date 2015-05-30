namespace ceenq.com.Core.Routing
{
    public interface IRoute {
        int Id { get; }
        string RequestPattern { get; set; }
        string PassTo { get; set; }
        int RouteOrder { get; set; }
        bool RequireAuthentication { get; set; }
        bool CachingEnabled { get; set; }
        string Rules { get; set; }
    }

    public class DefaultRouteImpl : IRoute
    {
        public int Id { get; private set; }
        public string RequestPattern { get; set; }
        public string PassTo { get; set; }
        public int RouteOrder { get; set; }
        public bool RequireAuthentication { get; set; }
        public bool CachingEnabled { get; set; }
        public string Rules { get; set; }
    }
}