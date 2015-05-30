using System;
using System.Collections.Generic;
using System.Linq;
using ceenq.com.Core.Assets;
using ceenq.com.Core.Extensions;
using Microsoft.WindowsAzure.Storage.File;

namespace ceenq.com.AzureCloudStorage {



    public class CloudStorageItem : IAssetDirectoryItem {
        private readonly CloudFileDirectory _root;
        private readonly CloudFileDirectory _directory;
        public CloudStorageItem(IListFileItem item, CloudFileDirectory root) {
            _root = root;
            Parent = new CloudStorageFolder(item.Parent, root);
            if (item is CloudFileDirectory)
            {
                Type = AssetDirectoryItemType.Directory;
                _directory = (CloudFileDirectory)item;
                var storageFolder = new CloudStorageFolder(_directory, _root);
                Name = storageFolder.Name;
                Id = storageFolder.Id;
                Path = storageFolder.Path;
            }
            else {
                Type = AssetDirectoryItemType.File;
                var file = ((CloudFile) item);
                var storageFile = new CloudStorageFile(file, _root);
                Name = storageFile.Name;
                Id = storageFile.Id;
                Path = storageFile.Path;
                ContentType = storageFile.ContentType;
            }
        }

        public AssetDirectoryItemType Type { get; private set; }
        public string Name { get; private set; }
        public string Path { get; private set; }
        public string Id { get; private set; }
        public IAssetFolder Parent { get; private set; }

        private IEnumerable<IAssetDirectoryItem> _children; 
        public IEnumerable<IAssetDirectoryItem> Children {
            get {
                if (Type != AssetDirectoryItemType.Directory)
                    return null;

                return _children ?? (_children = _directory.ListFilesAndDirectories().Select(item => new CloudStorageItem(item, _root)));
            }
        }

        public string ContentType { get; private set; }
    }
}