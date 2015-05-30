using ceenq.com.Assets.Services;
using ceenq.com.Core.Accounts;
using ceenq.com.Core.Environment;
using ceenq.com.Core.Models;
using JetBrains.Annotations;
using Orchard;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Validation;
using System;
using System.IO;
using System.Web;

namespace ceenq.com.AssetImport.Services
{
    public interface IContentImportService : IDependency
    {
        void ImportContent(string folderPath, bool overwriteExisting, HttpPostedFileBase postedFile);
        void ImportContent(string folderPath, bool overwriteExisting, byte[] bytes);
        void ImportContent(string folderPath, bool overwriteExisting, Stream inputStream);
    }

    [UsedImplicitly]
    public class ContentImportService :Component, IContentImportService
    {
        private readonly IAssetService _contentDocumentManager;


        public ContentImportService(IAssetService contentDocumentManager)
        {
            _contentDocumentManager = contentDocumentManager;
        }

        /// <summary>
        /// Uploads a media file based on a posted file.
        /// </summary>
        /// <param name="folderPath">The path to the folder where to upload the file.</param>
        /// <param name="overwriteExisting"></param>
        /// <param name="postedFile">The file to upload.</param>
        public void ImportContent(string folderPath, bool overwriteExisting, HttpPostedFileBase postedFile)
        {
            Argument.ThrowIfNullOrEmpty(folderPath, "folderPath");
            Argument.ThrowIfNull(postedFile, "postedFile");
            Argument.Validate(IsZipFile(Path.GetExtension(postedFile.FileName)), "postedFile", "Must be a zip file");


            ImportContent(folderPath, overwriteExisting, postedFile.InputStream);
        }

        /// <summary>
        /// Uploads a media file based on an array of bytes.
        /// </summary>
        /// <param name="folderPath">The path to the folder where to upload the file.</param>
        /// <param name="overwriteExisting"></param>
        /// <param name="bytes">The array of bytes with the file's contents.</param>
        public void ImportContent(string folderPath, bool overwriteExisting, byte[] bytes)
        {
            Argument.ThrowIfNullOrEmpty(folderPath, "folderPath");
            Argument.ThrowIfNull(bytes, "bytes");

            ImportContent(folderPath, overwriteExisting, new MemoryStream(bytes));
        }

        /// <summary>
        /// Uploads a media file based on a stream.
        /// </summary>
        /// <param name="folderPath">The folder path to where to upload the file.</param>
        /// <param name="overwriteExisting"></param>
        /// <param name="inputStream">The stream with the file's contents.</param>
        public void ImportContent(string folderPath, bool overwriteExisting, Stream inputStream)
        {
            Argument.ThrowIfNullOrEmpty(folderPath, "folderPath");
            Argument.ThrowIfNull(inputStream, "inputStream");

            ProcessZipFile(folderPath, overwriteExisting, inputStream);
        }




        /// <summary>
        /// Unzips a media archive file.
        /// </summary>
        /// <param name="targetFolder">The folder where to unzip the file.</param>
        /// <param name="overwriteExisting"></param>
        /// <param name="zipStream">The archive file stream.</param>
        protected void ProcessZipFile(string targetFolder, bool overwriteExisting, Stream zipStream)
        {

            targetFolder = targetFolder.Trim('/');
            if (!string.IsNullOrEmpty(targetFolder))
                targetFolder = string.Format("/{0}/", targetFolder);

            _contentDocumentManager.CreateFromZip(targetFolder, zipStream, overwriteExisting);
        }


        /// <summary>
        /// Determines if a file is a Zip Archive based on its extension.
        /// </summary>
        /// <param name="extension">The extension of the file to analyze.</param>
        /// <returns>True if the file is a Zip archive; false otherwise.</returns>
        private static bool IsZipFile(string extension)
        {
            return string.Equals(extension.TrimStart('.'), "zip", StringComparison.OrdinalIgnoreCase);
        }


    }
}