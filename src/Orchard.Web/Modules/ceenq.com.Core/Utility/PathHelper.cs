using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls.WebParts;

namespace ceenq.com.Core.Utility
{
    public static class PathHelper
    {

        public static string CleanPath(string path)
        {
            var pathPrefix = string.Empty;
            path = path.ToLower();
            if (path.StartsWith("http://"))
            {
                pathPrefix = "http://";
                path = path.Replace("http://", "");
            } 
            else if (path.StartsWith("https://"))
            {
                pathPrefix = "https://";
                path = path.Replace("https://", "");
            }
            var pathParts = path.Split(new[] {'\\', '/'},StringSplitOptions.RemoveEmptyEntries);
            var cleanPath = new StringBuilder(string.Empty);
            for(var i = 0; i < pathParts.Length; i++)
            {
                if (i > 0) cleanPath.Append('/');
                cleanPath.Append(pathParts[i]);
            }
            cleanPath.Insert(0, pathPrefix);
            if (!Uri.IsWellFormedUriString(cleanPath.ToString(), UriKind.RelativeOrAbsolute))
            {
                throw new ArgumentException(string.Format("The path provided is not a well formed uri.  path={0}", path), "path");
            }
            if (!Path.HasExtension(cleanPath.ToString()))
                cleanPath.Append('/');

            if (Uri.IsWellFormedUriString(cleanPath.ToString(), UriKind.Relative))
                cleanPath.Insert(0, '/');

            return cleanPath.ToString();
        }

        public static string CleanFile(string fileName)
        {
            if (!Path.HasExtension(fileName))
            {
                throw new ArgumentException(string.Format("The file name provided does not have an extension.  fileName={0}", fileName), "fileName");
            }
            return Path.GetFileName(fileName);
        }

        public static string Combine(params string[] pathSegments)
        {
            if (pathSegments.Length == 0) return "/";

            var path = new StringBuilder(string.Empty);
            foreach (var pathSegment in pathSegments)
            {
                path.Append(CleanPath(pathSegment));
            }
            return CleanPath(path.ToString());
        }

    }
}