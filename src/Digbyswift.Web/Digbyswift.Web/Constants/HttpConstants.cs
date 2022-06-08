using System.Net;

namespace Digbyswift.Web.Constants
{
    public static class HttpConstants
    {
        public static readonly HttpStatusCode[] RetryStatusCodes = {
            HttpStatusCode.Unauthorized, // 401
            HttpStatusCode.Forbidden, // 403
            HttpStatusCode.RequestTimeout, // 408
#if NETSTANDARD2_1
            HttpStatusCode.TooManyRequests, // 429
#else
            (HttpStatusCode) 429,
#endif
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
        
        public static class Headers
        {
            public const string Accept = "Accept";
            public const string AcceptEncoding = "Accept-Encoding";
            public const string AcceptLanguage = "Accept-Language";
            public const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
            public const string Age = "Age";
            public const string Authorization = "Authorization";
            public const string CacheControl = "Cache-Control";
            public const string ContentDisposition = "Content-Disposition";
            public const string ContentLength = "Content-Length";
            public const string ContentType = "Content-Type";
            public const string ContentLanguage = "Content-Language";
            public const string Cookie = "Cookie";
            public const string Etag = "ETag";
            public const string Expires = "Expires";
            public const string Host = "Host";
            public const string KeepAlive = "Keep-Alive";
            public const string LastModified = "Last-Modified";
            public const string Origin = "Origin";
            public const string Pragma = "Pragma";
            public const string Referrer = "Referer";
            public const string UserAgent = "User-Agent";
            public const string XmlHttpRequest = "XMLHttpRequest";
            public const string WwwAuthenticate = "WWW-Authenticate";
        }
    }
}