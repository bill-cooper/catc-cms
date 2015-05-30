using System.Collections.Generic;
using System.IO;
using Orchard.FileSystems.Media;

namespace ceenq.com.Core.Extensions
{
    public static class StorageProviderExtensions
    {
        public static IEnumerable<IStorageFile> ListFilesRecursive(this IStorageProvider storageProvider, string path)
        {

            foreach (var folder in storageProvider.ListFolders(path))
            {
                foreach (var file in storageProvider.ListFilesRecursive(folder.GetPath()))
                    yield return file;
            }

            foreach (var file in storageProvider.ListFiles(path)) yield return file;
        }
    }
}