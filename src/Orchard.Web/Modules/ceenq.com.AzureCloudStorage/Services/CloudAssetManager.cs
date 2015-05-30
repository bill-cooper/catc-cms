using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using ceenq.com.Core.Assets;
using ceenq.com.Core.Extensions;
using ceenq.com.Core.Infrastructure.Compute;
using ceenq.com.Core.Routing;
using Microsoft.WindowsAzure.Storage.File;
using Orchard;
using Orchard.Environment.Configuration;
using Orchard.Environment.Extensions;

namespace ceenq.com.AzureCloudStorage.Services
{
    [OrchardFeature("ceenq.com.CloudAssetManagement")]
    [OrchardSuppressDependency("ceenq.com.Core.Assets.DefaultAssetManager")]
    public class CloudAssetManager :Component, IAssetManager
    {
        private readonly Lazy<CloudFileDirectory> _root;
        private readonly Lazy<CloudFileClient> _fileClient;
        private readonly Lazy<IServerCommandClient> _serverCommandClient;
        private readonly IServerCommandProvider _serverCommandProvider;

        public CloudAssetManager(IAzureStorageAccountProvider azureStorageAccountProvider, ShellSettings shellSettings, IServerCommandProvider serverCommandProvider, IRoutingServerProvider routingServerProvider)
        {
            var accountName = shellSettings.Name;
            _serverCommandProvider = serverCommandProvider;
            _fileClient = new Lazy<CloudFileClient>(() => azureStorageAccountProvider.GetStorageAccount().CreateCloudFileClient());

            _root = new Lazy<CloudFileDirectory>(() =>
            {
                var share = _fileClient.Value.GetShareReference(accountName);
                share.CreateIfNotExists();
                return share.GetRootDirectoryReference();
            });

            _serverCommandClient = new Lazy<IServerCommandClient>(routingServerProvider.GetDefaultCommandClient);
        }


        /// <summary>
        /// Checks if the given file exists within the storage provider.
        /// </summary>
        /// <param name="path">The relative path within the storage provider.</param>
        /// <returns>True if the file exists; False otherwise.</returns>
        public bool FileExists(string path)
        {
            return _root.Value.GetFileReference(CleanPath(path)).Exists();
        }

        /// <summary>
        /// Retrieves a file within the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the file within the storage provider.</param>
        /// <returns>The file.</returns>
        /// <exception cref="ArgumentException">If the file is not found.</exception>
        public IAssetFile GetFile(string path)
        {
            var cleanPath = CleanPath(path);
            var file = _root.Value.GetFileReference(cleanPath);
            return new CloudStorageFile(file, _root.Value);
        }

        /// <summary>
        /// Retrieves a folder within the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the folder within the storage provider.</param>
        /// <returns>The folder.</returns>
        /// <exception cref="ArgumentException">If the folder is not found.</exception>
        public IAssetFolder GetFolder(string path)
        {
            var cleanPath = CleanPath(path);
            var directory = _root.Value.GetDirectoryReference(cleanPath);
            return new CloudStorageFolder(directory, _root.Value);
        }

        /// <summary>
        /// Retrieves a storage item (could be a file or folder) within the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the storage item within the storage provider.</param>
        /// <returns>The storage item.</returns>
        /// <exception cref="ArgumentException">If the storage item is not found.</exception>
        public IAssetDirectoryItem GetStorageItem(string path)
        {
            var cleanPath = CleanPath(path);
            if (Path.HasExtension(cleanPath))
            {
                var file = _root.Value.GetFileReference(cleanPath);
                return new CloudStorageItem(file, _root.Value);

            }
            var directory = _root.Value.GetDirectoryReference(cleanPath);
            return new CloudStorageItem(directory, _root.Value);
        }

        /// <summary>
        /// Creates a storage item (could be a file or folder) within the storage provider.
        /// </summary>
        /// <param name="path">The relative path of the storage item to create.</param>
        /// <returns>The storage item.</returns>
        /// <exception cref="ArgumentException">If the storage item is not found.</exception>
        public IAssetDirectoryItem CreateStorageItem(string path)
        {
            var cleanPath = CleanPath(path);
            if (Path.HasExtension(cleanPath))
            {
                var file = CreateCloudFile(path);
                return new CloudStorageItem(file, _root.Value);

            }
            var directory = CreateCloudFileDirectory(path);
            return new CloudStorageItem(directory, _root.Value);
        }

        /// <summary>
        /// Lists the file and directory within a storage provider's path.
        /// </summary>
        /// <param name="path">The relative path to the folder which items to list.</param>
        /// <returns>The list of file and directory in the directory.</returns>
        public IEnumerable<IAssetDirectoryItem> ListFilesAndDirectories(string path)
        {
            var cleanPath = CleanPath(path);
            if (string.IsNullOrEmpty(cleanPath))
            { //then provide items in the root directory
                return _root.Value.ListFilesAndDirectories().Select(item => new CloudStorageItem(item, _root.Value));
            }
            else
            {
                return _root.Value.GetDirectoryReference(CleanPath(path)).ListFilesAndDirectories().Select(item => new CloudStorageItem(item, _root.Value));
            }
        }

        /// <summary>
        /// Lists the files recursively storage provider's path.
        /// </summary>
        /// <param name="path">The relative path to the folder which items to list.</param>
        /// <returns>The list of files under the directory.</returns>
        public IEnumerable<IAssetDirectoryItem> ListFilesRecursive(string path)
        {
            var cleanPath = CleanPath(path);
            cleanPath = _root.Value.Share.Name + '/' + cleanPath.Replace("\\", "/");
            var result = _serverCommandClient.Value.ExecuteCommand(_serverCommandProvider.New<IListFilesCommand>(cleanPath));

            var filePaths = result.Message.Split(new []{'\n'}, StringSplitOptions.RemoveEmptyEntries);

            var cloudStorageItems = new List<CloudStorageItem>();
            foreach (var filePath in filePaths)
            {
                var cleanFilePath = filePath;
                if (cleanFilePath.StartsWith(_root.Value.Share.Name, StringComparison.OrdinalIgnoreCase))
                    cleanFilePath = cleanFilePath.Substring(_root.Value.Share.Name.Length);
                cleanFilePath = CleanPath(cleanFilePath);
                var file = _root.Value.GetFileReference(cleanFilePath);
                file.Properties.ContentType = MimeMapping.GetMimeMapping(cleanFilePath);
                cloudStorageItems.Add(new CloudStorageItem(file, _root.Value));
            }

            return cloudStorageItems;
        }

        /// <summary>
        /// Checks if the given directory exists within the storage provider.
        /// </summary>
        /// <param name="path">The relative path within the storage provider.</param>
        /// <returns>True if the directory exists; False otherwise.</returns>
        public bool FolderExists(string path)
        {
            return _root.Value.GetDirectoryReference(CleanPath(path)).Exists();
        }


        /// <summary>
        /// Creates a folder in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the folder to be created.</param>
        /// <exception cref="ArgumentException">If the folder already exists.</exception>
        public IAssetFolder CreateFolder(string path)
        {
            return new CloudStorageFolder(CreateCloudFileDirectory(path), _root.Value);
        }

        private CloudFileDirectory CreateCloudFileDirectory(string path)
        {
            var cleanPath = CleanPath(path);
            if (FolderExists(cleanPath)) _root.Value.GetDirectoryReference(cleanPath);
            CloudFileDirectory folder = null;
            var pathParts = cleanPath.Split('\\');
            var currentPath = new StringBuilder(string.Empty);
            foreach (var pathPart in pathParts)
            {
                if(currentPath.Length > 0)//skip the first pass
                    currentPath.Append('\\');
                currentPath.Append(pathPart);
                if (FolderExists(currentPath.ToString())) continue;
                folder = _root.Value.GetDirectoryReference(currentPath.ToString());
                folder.Create();
            }
            return folder;
        }

        /// <summary>
        /// Deletes a folder in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the folder to be deleted.</param>
        /// <exception cref="ArgumentException">If the folder doesn't exist.</exception>
        public void DeleteFolder(string path)
        {
            var cleanPath = CleanPath(path);
            var folder = _root.Value.GetDirectoryReference(cleanPath);
            DeleteRecursive(folder);
        }

        /// <summary>
        /// Deletes all items within a folder in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the folder in which the contents should be deleted.</param>
        /// <exception cref="ArgumentException">If the folder doesn't exist.</exception>
        public void DeleteFolderContents(string path)
        {
            CloudFileDirectory directory;
            var cleanPath = CleanPath(path);
            if (string.IsNullOrEmpty(cleanPath))
                directory = _root.Value;
            else
                directory = _root.Value.GetDirectoryReference(cleanPath);

            var children = directory.ListFilesAndDirectories().ToList();
            if (children.Any())
                foreach (var child in children)
                {
                    if (child is CloudFile)
                    {
                        ((CloudFile)child).Delete();
                    }
                    else if (child is CloudFileDirectory)
                    {
                        DeleteRecursive((CloudFileDirectory)child);
                    }
                }
        }

        private void DeleteRecursive(CloudFileDirectory directory)
        {
            var children = directory.ListFilesAndDirectories().ToList();
            if (children.Any())
                foreach (var child in children)
                {
                    if (child is CloudFile)
                    {
                        ((CloudFile)child).Delete();
                    }
                    else if (child is CloudFileDirectory)
                    {
                        DeleteRecursive((CloudFileDirectory)child);
                    }
                }
            directory.Delete();
        }

        /// <summary>
        /// Renames a file or folder in the storage provider.
        /// </summary>
        /// <param name="oldPath">The relative path to the file or folder to be renamed.</param>
        /// <param name="newPath">The relative path to the new file or folder.</param>
        public void Rename(string oldPath, string newPath)
        {
            Move(oldPath, newPath);
        }

        /// <summary>
        /// Moves a file or folder in the storage provider.
        /// </summary>
        /// <param name="oldPath">The relative path to the file or folder to be moved.</param>
        /// <param name="newPath">The relative path to the new file or folder location.</param>
        public void Move(string oldPath, string newPath)
        {
            var cleanOldPath = CleanPath(oldPath);
            var cleanNewPath = CleanPath(newPath);

            cleanOldPath = _root.Value.Share.Name + '/' + cleanOldPath.Replace("\\", "/");
            cleanNewPath = _root.Value.Share.Name + '/' + cleanNewPath.Replace("\\", "/");
            _serverCommandClient.Value.ExecuteCommand(_serverCommandProvider.New<IMoveCommand>(cleanOldPath, cleanNewPath));
        }


        /// <summary>
        /// Deletes a file in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the file to be deleted.</param>
        /// <exception cref="ArgumentException">If the file doesn't exist.</exception>
        public void DeleteFile(string path)
        {
            var cleanPath = CleanPath(path);
            var file = _root.Value.GetFileReference(cleanPath);
            if (file.Exists())
                file.Delete();
        }

        /// <summary>
        /// Tries to delete a file in the storage provider.  Failure ignored
        /// </summary>
        /// <param name="path">The relative path to the file to be deleted.</param>
        public bool TryDeleteFile(string path)
        {
            bool result = true;
            try
            {
                DeleteFile(path);
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Creates a file in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the file to be created.</param>
        /// <exception cref="ArgumentException">If the file already exists.</exception>
        /// <returns>The created file.</returns>
        public IAssetFile CreateFile(string path)
        {
            return new CloudStorageFile(CreateCloudFile(path), _root.Value);
        }

        /// <summary>
        /// Deletes an item in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the item to be deleted.</param>
        /// <exception cref="ArgumentException">If the item doesn't exist.</exception>
        public void DeleteDirectoryItem(string path) {
            var cleanPath = CleanPath(path);
            if (Path.HasExtension(cleanPath)) {
                DeleteFile(cleanPath);
            }
            else {
                DeleteFolder(cleanPath);
            }
        }

        /// <summary>
        /// Compresses a storage item.
        /// </summary>
        /// <param name="path">The relative path to the item to be compressed.</param>
        public IAssetDirectoryItem CompressStorageItem(string path)
        {
            var cleanPath = CleanPath(path);
            var zipFilePath = cleanPath.TrimEnd('\\') + ".zip";

            var fullSharePath = _root.Value.Share.Name + '/' + cleanPath.Replace("\\", "/");
            var fullShareZipPath = _root.Value.Share.Name + '/' + zipFilePath.Replace("\\", "/");

            _serverCommandClient.Value.ExecuteCommand(_serverCommandProvider.New<IZipCommand>(fullShareZipPath, fullSharePath));
            return GetStorageItem(zipFilePath);
        }


        private CloudFile CreateCloudFile(string path)
        {
            var cleanPath = CleanPath(path);
            var directoryPath = Path.GetDirectoryName(cleanPath);
            if (!string.IsNullOrEmpty(directoryPath) && directoryPath != "\\")
            {
                CreateFolder(directoryPath);
            }

            var file = _root.Value.GetFileReference(cleanPath);
            file.Properties.ContentType = MimeMapping.GetMimeMapping(cleanPath);
            file.Create(0);
            return file;
        }

        /// <summary>
        /// Gets the asset file based on an id.
        /// </summary>
        /// <param name="id">The id string.</param>
        public IAssetFile GetFileById(string id)
        {
            var path = id.FromCompressedBase62String();
            return GetFile(path);
        }

        /// <summary>
        /// Gets the directory item based on an id.
        /// </summary>
        /// <param name="id">The id string.</param>
        public IAssetDirectoryItem GetDirectoryItemById(string id)
        {
            var path = id.FromCompressedBase62String();
            return GetStorageItem(path);
        }

        /// <summary>
        /// Deletes an item based on the id.
        /// </summary>
        /// <param name="id">The id string.</param>
        public void DeleteById(string id)
        {
            var path = id.FromCompressedBase62String();
            DeleteDirectoryItem(path);
        }

        /// <summary>
        /// Renames an item based on the id.
        /// </summary>
        /// <param name="id">The id string.</param>
        /// <param name="newPath">The path gives the full path and name of the renamed asset.</param>
        public string RenameById(string id, string newPath)
        {
            var oldPath = id.FromCompressedBase62String();
            Rename(oldPath, newPath);
            var newId = newPath.ToCompressedBase62String();
            return newId;
        }

        /// <summary>
        /// Moves an item based on the id
        /// </summary>
        /// <param name="id">The id string.</param>
        /// <param name="newPath">The path where the asset should be moved.</param>
        public string MoveById(string id, string newPath)
        {
            var oldPath = id.FromCompressedBase62String();
            Move(oldPath, newPath);
            var newId = newPath.ToCompressedBase62String();
            return newId;
        }

        private string CleanPath(string path)
        {
            string cleanPath;
            var fileName = Path.GetFileName(path) ?? string.Empty;
            var directoryPath = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(directoryPath) || directoryPath == "\\")
            {
                cleanPath = fileName;
            }
            else
            {
                directoryPath = directoryPath.Trim('\\');
                cleanPath = string.Format("{0}\\{1}", directoryPath, fileName);
            }
            return cleanPath;
        }

    }
}