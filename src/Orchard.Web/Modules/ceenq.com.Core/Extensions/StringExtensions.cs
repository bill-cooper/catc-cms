using System;
using System.IO;
using System.IO.Compression;

namespace ceenq.com.Core.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Get a substring of the first N characters.
        /// </summary>
        public static string Truncate(this string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length);
            }
            return source;
        }
        public static string FromCompressedBase62String(this string str) {

            return UnZipStr(str.FromBase62());
        }

        public static string ToCompressedBase62String(this string str)
        {
            return ZipStr(str).ToBase62();
        }
        private static byte[] ZipStr(String str)
        {
            using (var output = new MemoryStream())
            {
                using (var gzip =
                  new DeflateStream(output, CompressionMode.Compress))
                {
                    using (var writer =
                      new StreamWriter(gzip, System.Text.Encoding.UTF8))
                    {
                        writer.Write(str);
                    }
                }

                return output.ToArray();
            }
        }

        private static string UnZipStr(byte[] input)
        {
            using (var inputStream = new MemoryStream(input))
            {
                using (var gzip =
                  new DeflateStream(inputStream, CompressionMode.Decompress))
                {
                    using (var reader =
                      new StreamReader(gzip, System.Text.Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }


}