using ceenq.com.Core.Assets;
using Microsoft.WindowsAzure.Storage;
using Orchard;

namespace ceenq.com.AzureCloudStorage.Services
{
    public interface IAzureStorageAccountProvider : IDependency
    {
        CloudStorageAccount GetStorageAccount();
    }
    public class AzureStorageAccountProvider : IAzureStorageAccountProvider
    {
        private readonly IAssetStorageCredentialsProvider _assetStorageCredentials;
        public AzureStorageAccountProvider(IAssetStorageCredentialsProvider assetStorageCredentials)
        {
            _assetStorageCredentials = assetStorageCredentials;
        }

        public CloudStorageAccount GetStorageAccount()
        {
            return CloudStorageAccount.Parse(
                    string.Format("DefaultEndpointsProtocol={0};AccountName={1};AccountKey={2}"
                    , "https"
                    , _assetStorageCredentials.Username
                    , _assetStorageCredentials.Password
                    )
                );
        }
    }
}