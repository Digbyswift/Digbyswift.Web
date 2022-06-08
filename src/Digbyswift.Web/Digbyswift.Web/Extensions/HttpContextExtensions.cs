#if NETSTANDARD2_1
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
#else
using System.Web;
#endif

namespace Digbyswift.Web.Extensions
{
    public static class HttpContextExtensions
    {
#if NETSTANDARD2_1
        private static ILogger _logger;

        public static ILogger GetLogger(this HttpContext httpContext)
        {
            if (_logger != null)
                return _logger;

            return _logger = httpContext.RequestServices.GetService<ILogger>();
        }
#endif

        public static bool IsLoggedIn(this HttpContext httpContext)
        {
            return httpContext.User.Identity is { IsAuthenticated: true };
        }
    }
}