using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebMarkupMin.AspNetCore5;
using WebMarkupMin.Core;

namespace Digbyswift.Web.Startup.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStartupTask<T>(this IServiceCollection services)
            where T : class, IStartupTask
            => services.AddTransient<IStartupTask, T>();

        public static IServiceCollection AddMarkupMinification(this IServiceCollection services, IConfiguration config)
        {
            if (config.GetValue<bool>("MarkupMinification:IsEnabled"))
            {
                services
                    .AddWebMarkupMin(options => options.AllowMinificationInDevelopmentEnvironment = true)
                    .AddHtmlMinification(options =>
                    {
                        options.MinificationSettings.WhitespaceMinificationMode = WhitespaceMinificationMode.Medium;
                        options.MinificationSettings.AttributeQuotesRemovalMode = HtmlAttributeQuotesRemovalMode.Html5;
                    });
            }

            return services;
        }


    }
}