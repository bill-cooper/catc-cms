using Orchard.Events;

namespace ceenq.com.Core.Assets
{
    public interface IAssetStorageEventHandler: IEventHandler
    {
        void AssetStorageCreated(AssetStorageEventContext context);
        void AssetStorageDeleted(AssetStorageEventContext context);
    }

    public class AssetStorageEventContext
    {

    }
}
