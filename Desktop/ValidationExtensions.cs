using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Desktop
{
    public static class ValidationExtensions
    {
        internal static bool CheckUrl(this string url)
        {
            Uri uriResult;

            return Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        internal static bool CheckDepth(this string depth)
        {
            int n;

            var result = int.TryParse(depth, out n);

            return result && n <= 50 && n > 0;
        }

        internal static bool CheckSource(this string path)
        {
            DirectoryInfo directory;
            try
            {
                directory = new DirectoryInfo(path);
            }
            catch (Exception)
            {
                return false;
            }
            return directory.Exists;
        }
    }
}
