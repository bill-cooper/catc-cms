using System;
using System.IO;
using System.Web;
using ceenq.com.Core.Assets;
using ceenq.com.Core.Extensions;
using Microsoft.WindowsAzure.Storage.File;

namespace ceenq.com.AzureCloudStorage
{
    public class CloudStorageFile : IAssetFile
    {
        private readonly CloudFile _file;
        private readonly CloudFileDirectory _root;

        public CloudStorageFile(CloudFile file, CloudFileDirectory root)
        {
            _file = file;
            _root = root;
        }

        public string Path
        {
            get
            {
                var path = _file.Uri.AbsolutePath;
                var shareName = string.Format("/{0}/", _file.Share.Name);
                if (path.StartsWith(shareName, StringComparison.OrdinalIgnoreCase))
                    path = path.Substring(shareName.Length);
                path = HttpUtility.UrlDecode(path);
                path = path.Replace('\\', '/');
                if (!path.StartsWith("/"))
                    path = "/" + path;
                return path;
            }
        }

        public IAssetFolder Parent
        {
            get
            {
                if (_file.Parent != null)
                {
                    return new CloudStorageFolder(_file.Parent, _root);
                }
                throw new ArgumentException("File " + _file.Uri + " does not have a parent directory");
            }
        }

        public string Name
        {
            get { return _file.Name; }
        }

        public string Id
        {
            get
            {
                return Path.ToCompressedBase62String();
            }
        }

        public string ContentType
        {
            get
            {
                return MimeMapping.GetMimeMapping(Name);
            }
        }

        public long GetSize()
        {
            EnsureAttributesLoaded();
            return _file.Properties.Length;
        }

        public DateTime GetLastUpdated()
        {
            EnsureAttributesLoaded();
            return _file.Properties.LastModified.GetValueOrDefault().DateTime;
        }

        public string GetFileType()
        {
            return System.IO.Path.GetExtension(Path);
        }

        public void Write(byte[] data)
        {
            _file.UploadFromByteArray(data, 0, data.Length);
        }

        public void WriteStream(Stream stream)
        {
            _file.UploadFromStream(stream);
        }
        public byte[] Read()
        {
            byte[] data;
            EnsureAttributesLoaded();
            using (var stream = new MemoryStream())
            {
                _file.DownloadToStream(stream);
                stream.Position = 0;
                using (var br = new BinaryReader(stream))
                {
                    data = br.ReadBytes((int)stream.Length);
                }
            }

            return data;
        }
        public Stream ReadStream()
        {
            EnsureAttributesLoaded();
            var stream = new MemoryStream();

            _file.DownloadToStream(stream);
            stream.Position = 0;
            
            return stream;

        }

        private void EnsureAttributesLoaded()
        {
            if (_file.Properties.ETag == null)
                _file.FetchAttributes();
        }

    }
}