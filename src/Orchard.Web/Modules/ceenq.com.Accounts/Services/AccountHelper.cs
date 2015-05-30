using System;
using ceenq.com.Core.Accounts;

namespace ceenq.com.Accounts.Services
{
    public class AccountHelper : IAccountHelper
    {
        public string CanonicalAccountName(string accountName)
        {
            if (string.IsNullOrWhiteSpace(accountName)) throw new ArgumentException("A canonical account name connot be created because accountName is null or whitespace.","accountName");
            return accountName.ToLower().Replace("_", "").Replace("-", "");
        }

    }
}