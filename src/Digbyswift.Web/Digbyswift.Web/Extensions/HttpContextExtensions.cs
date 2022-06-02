using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Digbyswift.Web.Extensions
{
    public static class HttpContextExtensions
    {
        private static ILogger _logger;

        public static ILogger GetLogger(this HttpContext httpContext)
        {
            if (_logger != null)
                return _logger;

            return _logger = httpContext.RequestServices.GetService<ILogger>();
        }

        public static bool IsLoggedIn(this HttpContext httpContext)
        {
            return httpContext.User.Identity is { IsAuthenticated: true };
        }
    }
}