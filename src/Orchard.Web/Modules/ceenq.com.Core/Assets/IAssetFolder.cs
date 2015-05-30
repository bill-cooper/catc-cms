namespace ceenq.com.Core.Assets {
    public interface IAssetFolder
    {
        string Path { get; }

        IAssetFolder Parent { get; }

        string Name { get; }

        string Id { get; }
        long GetSize();
    }
}