using ceenq.com.Core.Models;
using Orchard;

namespace ceenq.com.Core.Applications
{
    public interface IApplicationRequestContext : IDependency
    {
        IApplication Application { get; }
        bool IsApplicationRequest();
        bool IsAssetRequest();
        bool IsCmsRequest();
        string ApplicationRequestUrl();
    }
}
