using System.Net;
using System.Linq;
using Digbyswift.Web.Constants;
using Microsoft.AspNetCore.Http;

namespace Digbyswift.Web.Extensions
{
    public static class HttpResponseExtensions
    {
        private const string CacheControlValue = "no-cache, no-store, must-revalidate";
        private const string ExpiresValue = "-1";
        private const string PragmaValue = "no-cache";

        public static void SetNoCacheHeaders(this HttpResponse response)
        {
            response.Headers[HttpConstants.Headers.CacheControl] = CacheControlValue;
            response.Headers[HttpConstants.Headers.Expires] = ExpiresValue;
            response.Headers[HttpConstants.Headers.Pragma] = PragmaValue;
        }

        public static bool IsStatusCodeSuitableForRetry(this HttpResponse response)
        {
            return HttpConstants.RetryStatusCodes.Contains((HttpStatusCode)response.StatusCode);
        }
    }
}
