using System.Net;
using System.Net.Http;

namespace Digbyswift.Web.Mvc.Extensions
{
	public static class HttpResponseMessageExtensions
	{
        public static bool IsStatusCodeSuitableForRetry(this HttpResponseMessage message)
        {
            int statusCode = (int) message.StatusCode;
            return ((500 <= statusCode && statusCode < 600) ||
                    statusCode == (int) HttpStatusCode.RequestTimeout ||
                    statusCode == 429);
        }
	}
}