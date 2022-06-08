#if NETSTANDARD2_1
using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebMarkupMin.AspNetCore2;

namespace Digbyswift.Web.Startup.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomRewriteRules(this IApplicationBuilder builder, string filePath, Action<RewriteOptions> optionsAction)
        {
            if (String.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Cannot be null or empty", nameof(filePath));
            
            var options = new RewriteOptions();
            optionsAction(options);
            options.AddIISUrlRewrite(File.OpenText(filePath));

            return builder.UseRewriter(options);
        }

        public static IApplicationBuilder UseMarkupMinification(this IApplicationBuilder builder)
        {
            var config = builder.ApplicationServices.GetService<IConfiguration>();
            if (config.GetValue<bool>("MarkupMinification:IsEnabled"))
            {
                builder.UseWebMarkupMin();
            }

            return builder;
        }
    }
}
#endif