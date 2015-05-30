using System.Collections.Generic;
using ceenq.com.Assets.Models;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Assets;
using ceenq.com.Core.Utility;
using Ionic.Zip;
using Orchard;
using System.IO;
using System.Linq;
using System;
using Orchard.Logging;
using Orchard.UI.Notify;

namespace ceenq.com.Assets.Services
{
    public class AssetService : Component, IAssetService
    {
        private readonly IAccountContext _accountContext;
        private readonly IAssetManager _assetManager;
        private readonly INotifier _notifier;

        public AssetService(IAccountContext accountContext, IAssetManager assetManager, INotifier notifier)
        {
            _accountContext = accountContext;
            _assetManager = assetManager;
            _notifier = notifier;
        }
        public void CreateAsset(AssetModel assetModel)
        {
            var file = _assetManager.CreateFile(assetModel.Path);
            var encoding = new System.Text.UTF8Encoding(false);
            var data = encoding.GetBytes(assetModel.Body);
            file.Write(data);
        }

        public void CreateFromZip(string filePath, Stream zipStream, bool overwrite)
        {
            using (var fileInflater = ZipFile.Read(zipStream))
            {
                // We want to preserve whatever directory structure the zip file contained instead
                // of flattening it.
                // The API below doesn't necessarily return the entries in the zip file in any order.
                // That means the files in subdirectories can be returned as entries from the stream 
                // before the directories that contain them, so we create directories as soon as first
                // file below their path is encountered.
                var index = 0;
                foreach (var entry in fileInflater)
                {
                    index++;
                    if (entry == null) continue;
                    if (entry.IsDirectory || string.IsNullOrEmpty(entry.FileName)) continue;
                    if (!FileAllowed(entry.FileName)) continue;

                    using (var stream = entry.OpenReader())
                    {
                        try
                        {
                            var assetPath = _accountContext.ToInternalAssetPath(PathHelper.Combine(filePath, entry.FileName));
                            CreateAsset(assetPath, stream, overwrite);
                            Logger.Information("Successfully imported {0} - {1} of {2}", entry.FileName, index, fileInflater.Count);
                        }
                        catch (Exception ex)
                        {
                            _notifier.Error(T("Failed to import {0}", entry.FileName));
                            Logger.Error(ex, "Failed to import {0}", entry.FileName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Verifies if a file is allowed based on its name and the policies defined by the black / white lists.
        /// </summary>
        /// <param name="fileName">The file name of the file to validate.</param>
        /// <returns>True if the file is allowed; false if otherwise.</returns>
        private static bool FileAllowed(string fileName)
        {
            var file = Path.GetFileName(fileName);
            if (string.IsNullOrWhiteSpace(file)) return false;

            var localFileName = file.Trim();
            var extension = localFileName.Trim().TrimStart('.');

            if (string.IsNullOrEmpty(localFileName) || string.IsNullOrEmpty(extension)) return false;


            // TODO: implement a full blacklist
            if (string.Equals(localFileName, "web.config", StringComparison.OrdinalIgnoreCase)) return false;

            return true;
        }

        public void CreateAsset(string assetPath, Stream stream, bool overwrite)
        {
            if (_assetManager.FileExists(assetPath))
            {
                if (!overwrite) return;//no action
                UpdateAsset(assetPath, stream);
            }
            else
            {
                var file = _assetManager.CreateFile(assetPath);

                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    file.Write(ms.ToArray());
                }
            }
        }

        public DirectoryItemModel CompressDirectoryItem(DirectoryItemModel model)
        {
            var newStorageItem = _assetManager.CompressStorageItem(model.Path);
            return MapDirectoryItem(newStorageItem);
        }

        public void UpdateAsset(AssetModel assetModel)
        {
            var file = _assetManager.GetFileById(assetModel.Id);

            var encoding = new System.Text.UTF8Encoding(false);
            var data = encoding.GetBytes(assetModel.Body);
            file.Write(data);
        }

        public void UpdateAsset(string assetPath, Stream stream)
        {
            var file = _assetManager.CreateFile(assetPath);
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                file.Write(ms.ToArray());
            }
        }

        public DirectoryItemModel CreateDirectoryItem(DirectoryItemModel model)
        {
            var newStorageItem = _assetManager.CreateStorageItem(model.Path);
            return MapDirectoryItem(newStorageItem);
        }
        public DirectoryItemModel UpdateDirectoryItem(DirectoryItemModel model)
        {
            var newId = _assetManager.MoveById(model.Id, model.Path);
            return GetDirectoryItem(newId);
        }
        public void Delete(string id)
        {
            _assetManager.DeleteById(id);
        }
        public IEnumerable<AssetModel> GetAssets()
        {
            var assets = _assetManager.ListFilesRecursive("/").Select(MapAsset);

            return assets;
        }

        public IEnumerable<DirectoryItemModel> GetDirectoryList()
        {
            var items = _assetManager.ListFilesAndDirectories("/")
                .OrderByDescending(item => item.Type).Select(MapDirectoryItem);

            return items;
        }

        public IEnumerable<DirectoryItemModel> GetDirectoryListRecursive()
        {
            var items = _assetManager.ListFilesAndDirectories("/").OrderByDescending(item => item.Type).Select(item =>
            {
                var directoryItem = MapDirectoryItem(item);
                directoryItem.Children = BuildDirectoryItemList(item);
                return directoryItem;
            });

            return items;
        }

        private IEnumerable<DirectoryItemModel> BuildDirectoryItemList(IAssetDirectoryItem item)
        {
            if (item.Children == null) return new List<DirectoryItemModel>();
            return item.Children.OrderByDescending(child => child.Type).Select(child =>
            {
                var directoryItem = MapDirectoryItem(child);
                directoryItem.Children = BuildDirectoryItemList(child);
                return directoryItem;
            });

        }

        public DirectoryItemModel GetDirectoryItem(string id)
        {
            var storageItem = _assetManager.GetDirectoryItemById(id);
            var model = MapDirectoryItem(storageItem);

            if (storageItem.Type == AssetDirectoryItemType.Directory)
            {
                model.Children = storageItem.Children.OrderByDescending(item => item.Type).Select(MapDirectoryItem);
            }

            return model;
        }

        public AssetModel Get(string id)
        {
            var file = _assetManager.GetFileById(id);
            var model = new AssetModel { Path = file.Path, Id = id, ContentType = file.ContentType };

            var encoding = new System.Text.UTF8Encoding(false);
            var data = file.Read();
            model.Body = encoding.GetString(data);
            return model;
        }

        public Stream GetStream(string id)
        {
            var file = _assetManager.GetFileById(id);
            return file.ReadStream();
        }
        private static AssetModel MapAsset(IAssetDirectoryItem directoryItem)
        {
            return new AssetModel()
            {
                ContentType = directoryItem.ContentType,
                Id = directoryItem.Id,
                Path = directoryItem.Path
            };
        }
        private static DirectoryItemModel MapDirectoryItem(IAssetDirectoryItem directoryItem)
        {
            return new DirectoryItemModel()
            {
                ContentType = directoryItem.ContentType,
                Id = directoryItem.Id,
                Name = directoryItem.Name,
                Path = directoryItem.Path,
                Type = directoryItem.Type.ToString()
            };
        }
    }
}