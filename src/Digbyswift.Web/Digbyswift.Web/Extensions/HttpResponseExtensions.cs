using System.Net;
using System.Linq;
using Digbyswift.Web.Constants;
#if NETSTANDARD2_1
using Microsoft.AspNetCore.Http;
#else
using System.Web;
using Digbyswift.Core.Models;
#endif

namespace Digbyswift.Web.Extensions
{
    public static class HttpResponseExtensions
    {
#if NETSTANDARD2_1
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
#else
        public static HttpResponseBase AsBase(this HttpResponse response)
        {
            return new HttpResponseWrapper(response);
        }

        public static void SetNoCacheHeaders(this HttpResponse response)
        {
            response.AsBase().SetNoCacheHeaders();
        }

        public static void SetNoCacheHeaders(this HttpResponseBase response)
        {
            response.Cache.SetExpires(SystemTime.UtcNow().AddDays(-1));
            response.Cache.SetValidUntilExpires(false);
            response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();
        }

        public static bool IsStatusCodeSuitableForRetry(this HttpResponse response)
        {
            return response.AsBase().IsStatusCodeSuitableForRetry();
        }

        public static bool IsStatusCodeSuitableForRetry(this HttpResponseBase response)
        {
            return HttpConstants.RetryStatusCodes.Contains((HttpStatusCode)response.StatusCode);
        }
#endif
    }
}
