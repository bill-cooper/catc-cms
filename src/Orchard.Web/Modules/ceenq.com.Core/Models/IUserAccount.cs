using ceenq.com.Core.Accounts;
using Orchard.ContentManagement;

namespace ceenq.com.Core.Models
{
    public interface IUserAccount : IContent
    {
        IAccount Account { get; }
    }
}
