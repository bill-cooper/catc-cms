using System.Collections.Generic;
using Orchard;

namespace ceenq.com.Core.Accounts
{
    public interface IAccountManager : IDependency
    {
        IEnumerable<IAccount> GetAccounts();
        IAccount GetAccount(int id);
        IAccount GetAccountByName(string name);
        IAccount CreateAccount(string accountName);
        void DeleteAccount(int id);
    }
}