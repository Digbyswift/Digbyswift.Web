using System.Web;
using System.Linq;
using System.Net;
using Digbyswift.Core.Models;
using Digbyswift.Web.Net4.Constants;

namespace Digbyswift.Web.Net4.Extensions
{
    public static class HttpResponseExtensions
    {
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
    }
}
