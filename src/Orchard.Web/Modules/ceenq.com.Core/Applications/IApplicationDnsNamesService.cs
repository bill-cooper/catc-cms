using System.Collections.Generic;
using Orchard;

namespace ceenq.com.Core.Applications
{
    public interface IApplicationDnsNamesService : IDependency
    {
        List<string> DnsNames(IApplication application);
        string PrimaryDnsName(IApplication application);
        string DefaultDnsName(IApplication application);
    }
}