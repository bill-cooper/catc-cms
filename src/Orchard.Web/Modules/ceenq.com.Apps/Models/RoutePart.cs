using System.Collections.Generic;
using System.Linq;
using ceenq.com.Core.Applications;
using ceenq.com.Core.Models;
using ceenq.com.Core.Routing;
using Orchard.ContentManagement;

namespace ceenq.com.Apps.Models
{
    public class RoutePart : ContentPart<RouteRecord>, IRoute
    {
        public string RequestPattern
        {
            get { return Retrieve(x => x.RequestPattern); }
            set { Store(x => x.RequestPattern, value); }
        }
        public string PassTo
        {
            get { return Retrieve(x => x.PassTo); }
            set { Store(x => x.PassTo, value); }
        }
        public int RouteOrder
        {
            get { return Retrieve(x => x.RouteOrder); }
            set { Store(x => x.RouteOrder, value); }
        }
        public bool RequireAuthentication
        {
            get { return Retrieve(x => x.RequireAuthentication); }
            set { Store(x => x.RequireAuthentication, value); }
        }
        public bool CachingEnabled 
        {
            get { return Retrieve(x => x.CachingEnabled); }
            set { Store(x => x.CachingEnabled, value); }
        }
        public string Rules
        {
            get { return Retrieve(x => x.Rules); }
            set { Store(x => x.Rules, value); }
        }
        public int ApplicationId
        {
            get { return Retrieve(x => x.ApplicationId); }
            set { Store(x => x.ApplicationId, value); }
        }

        public static IList<IRoute> Sort(IEnumerable<RoutePart> routes)
        {
            return routes.OrderBy(r => r.RouteOrder).OfType<IRoute>().ToList();
        }
        public static IList<RoutePart> SortRouteParts(IEnumerable<RoutePart> routes)
        {
            return routes.OrderBy(r => r.RouteOrder).ToList();
        }
    }
}