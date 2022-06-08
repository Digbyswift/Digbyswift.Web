using System;
#if NETSTANDARD2_1        
using Microsoft.AspNetCore.Http;
#else
using System.Web;
#endif

namespace Digbyswift.Web.Extensions
{
	public static class StringValidationExtensions
    {
        
#if NETSTANDARD2_1        
        public static bool IsInternalUrl(this string url, IHttpContextAccessor httpContextAccessor)
        {
            if (String.IsNullOrWhiteSpace(url))
                return false;

            if (url.StartsWith("/") && Uri.TryCreate(url, UriKind.Relative, out var workingRelativeUri))
                return true;

            if (Uri.TryCreate(url, UriKind.Absolute, out Uri workingUri) && workingUri.IsBaseOf(httpContextAccessor.HttpContext.Request.GetRootUri()))
                return true;

            return false;
        }
#else        
        public static bool IsInternalUrl(this string url, HttpContext context)
        {
            if (String.IsNullOrWhiteSpace(url))
                return false;

            if (url.StartsWith("/") && Uri.TryCreate(url, UriKind.Relative, out var workingRelativeUri))
                return true;

            if (Uri.TryCreate(url, UriKind.Absolute, out Uri workingUri) && workingUri.DnsSafeHost.Equals(context.Request.Url.DnsSafeHost))
                return true;

            return false;
        }
#endif    
    }
}