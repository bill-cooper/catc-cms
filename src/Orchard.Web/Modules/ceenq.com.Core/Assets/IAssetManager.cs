using System;
using System.Collections.Generic;
using Orchard;
using Orchard.Security;

namespace ceenq.com.Core.Assets
{
    public interface IAssetManager : IDependency
    {
        /// <summary>
        /// Checks if the given file exists within the storage provider.
        /// </summary>
        /// <param name="path">The relative path within the storage provider.</param>
        /// <returns>True if the file exists; False otherwise.</returns>
        bool FileExists(string path);

        /// <summary>
        /// Retrieves a file within the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the file within the storage provider.</param>
        /// <returns>The file.</returns>
        /// <exception cref="ArgumentException">If the file is not found.</exception>
        IAssetFile GetFile(string path);

        /// <summary>
        /// Retrieves a folder within the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the folder within the storage provider.</param>
        /// <returns>The folder.</returns>
        /// <exception cref="ArgumentException">If the folder is not found.</exception>
        IAssetFolder GetFolder(string path);

        /// <summary>
        /// Retrieves a storage item (could be a file or folder) within the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the storage item within the storage provider.</param>
        /// <returns>The folder.</returns>
        /// <exception cref="ArgumentException">If the storage item is not found.</exception>
        IAssetDirectoryItem GetStorageItem(string path);

        /// <summary>
        /// Creates a storage item (could be a file or folder) within the storage provider.
        /// </summary>
        /// <param name="path">The relative path of the storage item to create.</param>
        /// <returns>The storage item.</returns>
        /// <exception cref="ArgumentException">If the storage item is not found.</exception>
        IAssetDirectoryItem CreateStorageItem(string path);
        /// <summary>
        /// Lists the file and directory within a storage provider's path.
        /// </summary>
        /// <param name="path">The relative path to the folder which items to list.</param>
        /// <returns>The list of file and directory in the directory.</returns>
        IEnumerable<IAssetDirectoryItem> ListFilesAndDirectories(string path);

        /// <summary>
        /// Lists the files recursively storage provider's path.
        /// </summary>
        /// <param name="path">The relative path to the folder which items to list.</param>
        /// <returns>The list of files under the directory.</returns>
        IEnumerable<IAssetDirectoryItem> ListFilesRecursive(string path);

        /// <summary>
        /// Checks if the given directory exists within the storage provider.
        /// </summary>
        /// <param name="path">The relative path within the storage provider.</param>
        /// <returns>True if the directory exists; False otherwise.</returns>
        bool FolderExists(string path);


        /// <summary>
        /// Creates a folder in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the folder to be created.</param>
        /// <exception cref="ArgumentException">If the folder already exists.</exception>
        IAssetFolder CreateFolder(string path);

        /// <summary>
        /// Deletes a folder in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the folder to be deleted.</param>
        /// <exception cref="ArgumentException">If the folder doesn't exist.</exception>
        void DeleteFolder(string path);

        /// <summary>
        /// Deletes all items within a folder in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the folder in which the contents should be deleted.</param>
        /// <exception cref="ArgumentException">If the folder doesn't exist.</exception>
        void DeleteFolderContents(string path);

        /// <summary>
        /// Renames a file or folder in the storage provider.
        /// </summary>
        /// <param name="oldPath">The relative path to the file or folder to be renamed.</param>
        /// <param name="newPath">The relative path to the new file or folder.</param>
        void Rename(string oldPath, string newPath);

        /// <summary>
        /// Moves a file or folder in the storage provider.
        /// </summary>
        /// <param name="oldPath">The relative path to the file or folder to be moved.</param>
        /// <param name="newPath">The relative path to the new file or folder location.</param>
        void Move(string oldPath, string newPath);

        /// <summary>
        /// Deletes a file in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the file to be deleted.</param>
        /// <exception cref="ArgumentException">If the file doesn't exist.</exception>
        void DeleteFile(string path);


        /// <summary>
        /// Tries to delete a file in the storage provider.  Failure ignored.
        /// </summary>
        /// <param name="path">The relative path to the file to be deleted.</param>
        bool TryDeleteFile(string path);

        /// <summary>
        /// Creates a file in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the file to be created.</param>
        /// <exception cref="ArgumentException">If the file already exists.</exception>
        /// <returns>The created file.</returns>
        IAssetFile CreateFile(string path);

        /// <summary>
        /// Deletes an item in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the item to be deleted.</param>
        /// <exception cref="ArgumentException">If the item doesn't exist.</exception>
        void DeleteDirectoryItem(string path);

        /// <summary>
        /// Gets the asset file based on an id.
        /// </summary>
        /// <param name="id">The id string.</param>
        IAssetFile GetFileById(string id);

        /// <summary>
        /// Gets the directory item based on an id.
        /// </summary>
        /// <param name="id">The id string.</param>
        IAssetDirectoryItem GetDirectoryItemById(string id);

        /// <summary>
        /// Deletes an item based on the id.
        /// </summary>
        /// <param name="id">The id string.</param>
        void DeleteById(string id);

        /// <summary>
        /// Renames an item based on the id.
        /// </summary>
        /// <param name="id">The id string.</param>
        /// <param name="newPath">The path gives the full path and name of the renamed asset.</param>
        string RenameById(string id, string newPath);

        /// <summary>
        /// Moves an item based on the id
        /// </summary>
        /// <param name="id">The id string.</param>
        /// <param name="newPath">The path where the asset should be moved.</param>
        string MoveById(string id, string newPath);

        /// <summary>
        /// Compresses a storage item.
        /// </summary>
        /// <param name="path">The relative path to the item to be compressed.</param>
        IAssetDirectoryItem CompressStorageItem(string path);
    }

    public class DefaultAssetManager :DefaultImplementationNotifier, IAssetManager
    {
        public DefaultAssetManager(IWorkContextAccessor workContextAccessor, IAuthenticationService authenticationService) : base(workContextAccessor, authenticationService)
        {
        }

        public bool IsInitialized { get; private set; }
        public void Initialize(string storageName)
        {
            throw new NotImplementedException();
        }

        public bool FileExists(string path)
        {
            throw new NotImplementedException();
        }

        public IAssetFile GetFile(string path)
        {
            throw new NotImplementedException();
        }

        public IAssetFolder GetFolder(string path)
        {
            throw new NotImplementedException();
        }

        public IAssetDirectoryItem GetStorageItem(string path)
        {
            throw new NotImplementedException();
        }

        public IAssetDirectoryItem CreateStorageItem(string path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAssetDirectoryItem> ListFilesAndDirectories(string path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAssetDirectoryItem> ListFilesRecursive(string path)
        {
            throw new NotImplementedException();
        }

        public bool FolderExists(string path)
        {
            throw new NotImplementedException();
        }

        public IAssetFolder CreateFolder(string path)
        {
            throw new NotImplementedException();
        }

        public void DeleteFolder(string path)
        {
            throw new NotImplementedException();
        }

        public void DeleteFolderContents(string path)
        {
            throw new NotImplementedException();
        }

        public void Rename(string oldPath, string newPath)
        {
            throw new NotImplementedException();
        }

        public void Move(string oldPath, string newPath)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(string path)
        {
            throw new NotImplementedException();
        }

        public bool TryDeleteFile(string path)
        {
            throw new NotImplementedException();
        }

        public IAssetFile CreateFile(string path)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectoryItem(string path)
        {
            throw new NotImplementedException();
        }

        public IAssetFile GetFileById(string id)
        {
            throw new NotImplementedException();
        }

        public IAssetDirectoryItem GetDirectoryItemById(string id)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(string id)
        {
            throw new NotImplementedException();
        }

        public string RenameById(string id, string newPath)
        {
            throw new NotImplementedException();
        }

        public string MoveById(string id, string newPath)
        {
            throw new NotImplementedException();
        }

        public IAssetDirectoryItem CompressStorageItem(string path)
        {
            throw new NotImplementedException();
        }
    }
}