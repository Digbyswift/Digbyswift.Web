using System;
using System.Linq;
using System.Net.Http;
using Digbyswift.Web.Net4.Constants;

namespace Digbyswift.Web.Net4.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        private const string CacheControlValue = "no-cache, no-store, must-revalidate";
        private const string ExpiresValue = "-1";
        private const string PragmaValue = "no-cache";

        public static void SetNoCacheHeaders(this HttpResponseMessage response)
        {
            // response.Headers.CacheControl = CacheControlValue;
            // response.Headers.[HttpConstants.Headers.Expires] = ExpiresValue;
            // response.Headers.Pragma = PragmaValue;
        }

        public static bool IsStatusCodeSuitableForRetry(this HttpResponseMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            return HttpConstants.RetryStatusCodes.Contains(message.StatusCode);
        }
    }
}
