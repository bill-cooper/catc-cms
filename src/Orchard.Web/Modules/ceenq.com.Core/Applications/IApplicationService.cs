using System.Collections.Generic;
using ceenq.com.Core.Models;
using ceenq.com.Core.Routing;
using Orchard;

namespace ceenq.com.Core.Applications
{
    public interface IApplicationService : IDependency
    {
        string ToInternalApplicationPath(IApplication application, string path);
        string ToInternalAbsoluteApplicationPath(IApplication application, string path);
        string ToPublicApplicationPath(IApplication application, string path);
        string ToPublicAbsoluteApplicationPath(IApplication application, string path);
        string ApplicationUrl(IApplication application);
        IList<IRoute> GetRoutes(IApplication application);
        string BaseDirectory(IApplication application);
    }
}