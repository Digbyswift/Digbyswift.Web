using System;
using System.Linq;
using System.Web;
using System.Web.Routing;

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

        public static string ReplaceQueryString(this Uri uri, object newValues)
        {
            var values = new RouteValueDictionary(newValues);
            var query = HttpUtility.ParseQueryString(uri.Query);
            foreach (var key in values.Keys)
            {
                query[key] = values[key].ToString();
            }

            var builder = new UriBuilder(uri)
            {
                Query = string.Join("&", query.AllKeys.Select(x => string.Join("=", x, query[(string)x].ToString())))
            };
            return builder.ToString();
        }
    }
}