using System;
using System.Web;
using ceenq.com.Core.Assets;
using ceenq.com.Core.Extensions;
using Microsoft.WindowsAzure.Storage.File;

namespace ceenq.com.AzureCloudStorage
{
    public class CloudStorageFolder : IAssetFolder
    {
        private readonly CloudFileDirectory _directory;
        private readonly CloudFileDirectory _root;

        public CloudStorageFolder(CloudFileDirectory directory, CloudFileDirectory root)
        {
            _directory = directory;
            _root = root;
        }

        public string Path
        {
            get
            {
                var path = _directory.Uri.AbsolutePath;
                var shareName = string.Format("/{0}/", _directory.Share.Name);
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
                if (_directory.Parent != null)
                {
                    return new CloudStorageFolder(_directory.Parent, _root);
                }
                throw new ArgumentException("Directory " + _directory.Uri + " does not have a parent directory");
            }
        }
        public string Name
        {
            get { return _directory.Name; }
        }

        public string Id
        {
            get
            {
                return Path.ToCompressedBase62String();
            }
        }

        public long GetSize()
        {
            return GetDirectorySize(_directory);
        }


        private static long GetDirectorySize(CloudFileDirectory directory)
        {
            long size = 0;

            foreach (IListFileItem item in directory.ListFilesAndDirectories())
            {
                if (item is CloudFile)
                    size += ((CloudFile)item).Properties.Length;

                if (item is CloudFileDirectory)
                    size += GetDirectorySize((CloudFileDirectory)item);
            }

            return size;
        }
    }
}