using System.Collections.Generic;

namespace ceenq.com.Core.Assets
{
    public interface IAssetDirectoryItem
    {
        AssetDirectoryItemType Type { get; }
        string Name { get; }
        string Path { get; }
        string Id { get; }
        IAssetFolder Parent { get; }
        IEnumerable<IAssetDirectoryItem> Children { get; }
        string ContentType { get; }
    }

    public enum AssetDirectoryItemType
    {
        File,
        Directory
    }
}