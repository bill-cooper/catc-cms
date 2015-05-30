using Orchard;

namespace ceenq.com.Core.Accounts {
    public interface IAccountHelper : IDependency {
        string CanonicalAccountName(string account);
    }
}