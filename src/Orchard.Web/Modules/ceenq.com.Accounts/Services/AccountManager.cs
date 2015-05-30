using System.Collections.Generic;
using System.Linq;
using ceenq.com.Core.Accounts;
using JetBrains.Annotations;
using Orchard;
using Orchard.ContentManagement;
using ceenq.com.Accounts.Models;

namespace ceenq.com.Accounts.Services
{
    [UsedImplicitly]
    public class AccountManager :Component, IAccountManager
    {
        private readonly IOrchardServices _orchardServices;

        public AccountManager(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
        }

        IAccount IAccountManager.GetAccount(int id)
        {
            var account = GetAccount(id);
            return account;
        }
        AccountPart GetAccount(int id)
        {
            var account = _orchardServices.ContentManager.Query<AccountPart, AccountRecord>().Where(u => u.Id == id).List().FirstOrDefault();
            return account;
        }
        IAccount IAccountManager.GetAccountByName(string name)
        {
            var account = _orchardServices.ContentManager.Query<AccountPart, AccountRecord>().Where(u => u.Name == name).List().FirstOrDefault();
            return account;
        }
        IEnumerable<IAccount> IAccountManager.GetAccounts()
        {
            return _orchardServices.ContentManager.Query<AccountPart, AccountRecord>().List();
        }
        IAccount IAccountManager.CreateAccount(string accountName)
        {
            var account = _orchardServices.ContentManager.New<AccountPart>("Account");
            account.DisplayName = accountName;
            _orchardServices.ContentManager.Create(account.ContentItem, VersionOptions.Published);

            return account;
        }
        public void DeleteAccount(int id) {
            var account = GetAccount(id);
            _orchardServices.ContentManager.Remove(account.ContentItem);
        }
    }
}