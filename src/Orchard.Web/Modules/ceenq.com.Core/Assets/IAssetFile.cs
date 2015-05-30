using System;
using System.IO;

namespace ceenq.com.Core.Assets {
    public interface IAssetFile
    {
        string Path { get; }

        IAssetFolder Parent { get;}

        string Name { get;}

        string Id { get; }

        string ContentType { get; }

        long GetSize();

        DateTime GetLastUpdated();

        string GetFileType();

        void Write(byte[] data);

        void WriteStream(Stream stream);

        byte[] Read();
        Stream ReadStream();

    }
}