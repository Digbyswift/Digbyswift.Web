using System;
#if NETSTANDARD2_1
using Microsoft.AspNetCore.Html;
#else
using System.Web;
#endif

namespace Digbyswift.Web.Extensions
{
    public static class StringExtensions
    {

#if NETSTANDARD2_1
        public static HtmlString AsRichText(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return null;

            return new HtmlString(value.Replace("\n", "<br />"));
        }
#else
        public static IHtmlString AsRichText(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return null;

            return new HtmlString(value.Replace("\n", "<br />"));
        }
#endif

        public static string EnsureScheme(this string value, bool isHttps = true)
        {
            if (value == null)
                return null;

            if (String.IsNullOrWhiteSpace(value))
                return value;

            int i = value.IndexOf("://", StringComparison.OrdinalIgnoreCase);
            if (i > -1)
                value = value.Substring(i + 3);

            return $"{(isHttps ? "https://" : "http://")}{value}";
        }

    }
}
