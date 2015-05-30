using System.Collections.Generic;
using ceenq.com.Assets.Models;
using ceenq.com.Core.Models;
using Orchard;
using System.IO;

namespace ceenq.com.Assets.Services
{
    public interface IAssetService : IDependency
    {
        void CreateAsset(AssetModel assetModel);
        void CreateAsset(string filePath, Stream stream, bool overwrite);
        void UpdateAsset(AssetModel assetModel);
        void UpdateAsset(string filePath, Stream stream);
        DirectoryItemModel CreateDirectoryItem(DirectoryItemModel model);
        void CreateFromZip(string filePath, Stream zipStream, bool overwrite);
        DirectoryItemModel UpdateDirectoryItem(DirectoryItemModel model);
        AssetModel Get(string id);
        Stream GetStream(string id);
        DirectoryItemModel GetDirectoryItem(string id);
        void Delete(string id);
        IEnumerable<AssetModel> GetAssets();
        IEnumerable<DirectoryItemModel> GetDirectoryList();
        DirectoryItemModel CompressDirectoryItem(DirectoryItemModel model);
    }
}
