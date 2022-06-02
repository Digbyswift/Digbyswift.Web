using System;

namespace Digbyswift.Web.Extensions
{
    public static class UriExtensions
    {
        public static string BaseUrl(this Uri uri)
        {
            return uri.GetLeftPart(UriPartial.Authority);
        }

        public static Uri BaseUri(this Uri uri)
        {
            return new Uri(uri.GetLeftPart(UriPartial.Authority));
        }

        public static string AbsoluteUrlPathOnly(this Uri uri)
        {
            return $"{uri.GetLeftPart(UriPartial.Authority)}{uri.AbsolutePath}";
        }
    }
}