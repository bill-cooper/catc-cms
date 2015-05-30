using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ceenq.com.Core.Utility
{
    public static class UrlHelper
    {
        public static bool IsValidUrl(string url)
        {
            return IsValidRelativeUrl(url) || IsValidAbsoluteUrl(url);
        }

        public static bool IsValidRelativeUrl(string url)
        {
            return Uri.IsWellFormedUriString(url, UriKind.Relative);
        }

        public static bool IsFilePath(string url)
        {
            if (IsValidRelativeUrl(url))
                return Path.HasExtension(url);

            var partialUrl = url.Replace("http://", "").Replace("https://", "");
            partialUrl = partialUrl.Trim('/');
            //if the partial path still contains a slash, then that means
            // that the path refers to a sub directory and still has a chance
            // of being a file link
            if (partialUrl.Contains("/") && Path.HasExtension(partialUrl))
                return true;

            //if not then this must not be a file link
            return false;
        }

        public static bool IsValidAbsoluteUrl(string url)
        {
            const string pattern = "^" +
                // protocol identifier
                                   "(?:(?:https?|ftp)://)" +
                // user:pass authentication
                                   "(?:\\S+(?::\\S*)?@)?" +
                                   "(?:" +
                // IP address exclusion
                // private & local networks
                                   "(?!10(?:\\.\\d{1,3}){3})" +
                                   "(?!127(?:\\.\\d{1,3}){3})" +
                                   "(?!169\\.254(?:\\.\\d{1,3}){2})" +
                                   "(?!192\\.168(?:\\.\\d{1,3}){2})" +
                                   "(?!172\\.(?:1[6-9]|2\\d|3[0-1])(?:\\.\\d{1,3}){2})" +
                // IP address dotted notation octets
                // excludes loopback network 0.0.0.0
                // excludes reserved space >= 224.0.0.0
                // excludes network & broacast addresses
                // (first & last IP address of each class)
                                   "(?:[1-9]\\d?|1\\d\\d|2[01]\\d|22[0-3])" +
                                   "(?:\\.(?:1?\\d{1,2}|2[0-4]\\d|25[0-5])){2}" +
                                   "(?:\\.(?:[1-9]\\d?|1\\d\\d|2[0-4]\\d|25[0-4]))" +
                                   "|" +
                // host name
                                   "(?:(?:[a-z\\u00a1-\\uffff0-9]+-?)*[a-z\\u00a1-\\uffff0-9]+)" +
                // domain name
                                   "(?:\\.(?:[a-z\\u00a1-\\uffff0-9]+-?)*[a-z\\u00a1-\\uffff0-9]+)*" +
                // TLD identifier
                                   "(?:\\.(?:[a-z\\u00a1-\\uffff]{2,}))" +
                                   ")" +
                // port number
                                   "(?::\\d{2,5})?" +
                // resource path
                                   "(?:/[^\\s]*)?" +
                                   "$";

            return Regex.IsMatch(url.Trim(), pattern, RegexOptions.IgnoreCase);
        }
    }
}