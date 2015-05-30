using System;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Assets;

namespace ceenq.com.AzureCloudStorage
{
    public class AccountCreationEventHandler : IAccountCreationEventHandler
    {
        private readonly IAssetStorageManager _assetStorageManager;
        public AccountCreationEventHandler(IAssetStorageManager assetStorageManager)
        {
            _assetStorageManager = assetStorageManager;
        }

        public void Initialize(AccountCreationEventContext context)
        {
            _assetStorageManager.CreateStorage(context.Account.Name);
        }

        public void Created(AccountCreationEventContext context)
        {
        }

        public void PostCreation(AccountCreationEventContext context)
        {
        }
    }
}