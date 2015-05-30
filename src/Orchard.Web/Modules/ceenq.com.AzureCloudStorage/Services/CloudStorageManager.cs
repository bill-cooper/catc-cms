using ceenq.com.Core.Assets;
using Orchard.Environment.Extensions;

namespace ceenq.com.AzureCloudStorage.Services
{
    [OrchardFeature("ceenq.com.AzureCloudStorageManagement")]
    [OrchardSuppressDependency("ceenq.com.Core.Assets.DefaultAssetStorageManager")]
    public class CloudStorageManager : IAssetStorageManager
    {
        private readonly IAzureStorageAccountProvider _azureStorageAccountProvider;
        private readonly IAssetStorageEventHandler _assetStorageEventHandler;
        public CloudStorageManager(IAzureStorageAccountProvider azureStorageAccountProvider, IAssetStorageEventHandler assetStorageEventHandler)
        {
            _azureStorageAccountProvider = azureStorageAccountProvider;
            _assetStorageEventHandler = assetStorageEventHandler;
        }

        public bool StorageExists(string name)
        {
            var fileClient = _azureStorageAccountProvider.GetStorageAccount().CreateCloudFileClient();
            var share = fileClient.GetShareReference(name);
            return share.Exists();
        }

        public void CreateStorage(string name)
        {
            var fileClient = _azureStorageAccountProvider.GetStorageAccount().CreateCloudFileClient();
            var share = fileClient.GetShareReference(name);
            share.CreateIfNotExists();
            _assetStorageEventHandler.AssetStorageCreated(new AssetStorageEventContext());
        }

        public void DeleteStorage(string name)
        {
            var fileClient = _azureStorageAccountProvider.GetStorageAccount().CreateCloudFileClient();
            var share = fileClient.GetShareReference(name);
            if (share.Exists())
                share.Delete();
            _assetStorageEventHandler.AssetStorageDeleted(new AssetStorageEventContext());
        }


    }
}