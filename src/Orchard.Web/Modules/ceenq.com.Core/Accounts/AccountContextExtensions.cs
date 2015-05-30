using System;
using ceenq.com.Core.Utility;
using Orchard;

namespace ceenq.com.Core.Accounts
{
    public static class AccountContextExtensions
    {
        public static string ToInternalAssetPath(this IAccountContext accountContext, string path)
        {
            if (PathHelper.CleanPath(path).StartsWith(accountContext.AssetPath))
                return PathHelper.CleanPath(path);
            return PathHelper.Combine(accountContext.AssetPath, PathHelper.CleanPath(path));
        }

        public static string ToServerAssetPath(this IAccountContext accountContext, string path)
        {
            return string.Format("{0}{1}", accountContext.ServerAbsoluteBasePath, ToInternalAssetPath(accountContext, path));
        }

        public static string ToInternalAbsoluteAssetPath(this IAccountContext accountContext, string path)
        {
            return string.Format("{0}{1}", accountContext.AbsoluteAccountBaseUrl, ToInternalAssetPath(accountContext, path));
        }

        public static string ToPublicAssetPath(this IAccountContext accountContext, string path)
        {
            path = PathHelper.CleanPath(path);
            var contentPath = PathHelper.CleanPath(accountContext.AssetPath);
            if (path.StartsWith(contentPath, StringComparison.OrdinalIgnoreCase))
                path = path.Substring(contentPath.Length);

            return PathHelper.CleanPath(path);
        }

        public static string ToPublicCmsPath(this IAccountContext accountContext, string path)
        {
            path = PathHelper.CleanPath(path);
            var contentPath = PathHelper.CleanPath(accountContext.CmsPath);
            if (path.StartsWith(contentPath, StringComparison.OrdinalIgnoreCase))
                path = path.Substring(contentPath.Length);
            path = PathHelper.CleanPath(path).Trim('/');
            return string.Format("/{0}", path);
        }

        public static string ToAbsoluteCmsPath(this IAccountContext accountContext, string path)
        {
            return PathHelper.CleanPath(path.Replace(accountContext.CmsToken, accountContext.InternalAbsoluteCmsPath));
        }

        public static string ToAbsoluteCmsAdminPath(this IAccountContext accountContext, string path)
        {
            return
                PathHelper.CleanPath(path.Replace(accountContext.CmsAdminToken,
                    accountContext.InternalAbsoluteCmsAdminPath));
        }

        public static string ToAbsoluteModulePath(this IAccountContext accountContext, string path)
        {
            if (path.StartsWith(accountContext.CmsAdminToken))
                return ToAbsoluteCmsAdminPath(accountContext, path);
            if (path.StartsWith(accountContext.CmsToken))
                return ToAbsoluteCmsPath(accountContext, path);
            throw new Exception("No module was found to match the token");
            //TODO: Do this better
        }
    }
}