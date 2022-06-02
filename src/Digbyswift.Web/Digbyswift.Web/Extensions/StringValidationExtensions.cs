using System;
using Microsoft.AspNetCore.Http;

namespace Digbyswift.Web.Extensions
{
	public static class StringValidationExtensions
    {
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
	}
}