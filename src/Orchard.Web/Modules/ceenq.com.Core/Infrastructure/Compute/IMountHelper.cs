using Orchard;

namespace ceenq.com.Core.Infrastructure.Compute
{
    public interface IMountHelper : IDependency
    {
        void EnsureMount(IServerCommandClient commandClient, string shareName, string mountDirectory);
    }
}