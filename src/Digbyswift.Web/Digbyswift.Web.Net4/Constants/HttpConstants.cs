using System.Net;

namespace Digbyswift.Web.Net4.Constants
{
    public static class HttpConstants
    {
        public static readonly HttpStatusCode[] RetryStatusCodes = {
            HttpStatusCode.Unauthorized, // 401
            HttpStatusCode.Forbidden, // 403
            HttpStatusCode.RequestTimeout, // 408
            HttpStatusCode.InternalServerError, // 500
            HttpStatusCode.BadGateway, // 502
            HttpStatusCode.ServiceUnavailable, // 503
            HttpStatusCode.GatewayTimeout, // 504
        };

        public static class Methods
        {
            public const string Get = "GET";
            public const string Post = "POST";
            public const string Head = "HEAD";
            public const string Options = "OPTIONS";
            public const string Put = "PUT";
            public const string Delete = "DELETE";
        }
    }
}