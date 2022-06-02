using System;
using System.Linq;
using System.Net.Http;
using Digbyswift.Web.Constants;

namespace Digbyswift.Web.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static bool IsStatusCodeSuitableForRetry(this HttpResponseMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            return HttpConstants.RetryStatusCodes.Contains(message.StatusCode);
        }
    }
}
