using Orchard.ContentManagement;

namespace ceenq.com.Core.Accounts
{
    public interface IAccount : IContent
    {
        string Name { get; set; }
        string DisplayName { get; set; }
        string Domain { get; set; }
    }
}
