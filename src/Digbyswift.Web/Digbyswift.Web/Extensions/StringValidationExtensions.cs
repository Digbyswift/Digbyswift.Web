using System;
using System.Web;

namespace Digbyswift.Web.Extensions
{
	public static class StringValidationExtensions
    {
        internal static Func<HttpContextBase> Context = () => new HttpContextWrapper(HttpContext.Current);
        
        /// <summary>
        /// Can only be used within a HttpContext request
        /// </summary>
        public static bool IsInternalUrl(this string url)
        {
            if (String.IsNullOrWhiteSpace(url))
                return false;

            if (url.StartsWith("/"))
                return true;

            if (Uri.TryCreate(url, UriKind.Absolute, out Uri workingUri) && workingUri.DnsSafeHost.Equals(Context().Request.Url.DnsSafeHost))
                return true;

            return false;
        }

	}
}