using System.IO;
using Digbyswift.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebMarkupMin.AspNetCore5;

namespace Digbyswift.Web.Startup.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomRewriteRules(this IApplicationBuilder builder)
        {
            using var iisUrlRewriteStreamReader = File.OpenText("rewriterules.config");
            var options = new RewriteOptions();
            options.Add(x =>
            {
                if (x.HttpContext.Request.IsPostMethod())
                {
                    x.Result = RuleResult.SkipRemainingRules;
                }
            });
            options.AddIISUrlRewrite(iisUrlRewriteStreamReader);

            return builder.UseRewriter(options);
        }

        public static IApplicationBuilder UseMarkupMinification(this IApplicationBuilder builder)
        {
            var isEnabled = builder.ApplicationServices.GetService<IConfiguration>().GetValue<bool>("MarkupMinification:IsEnabled");
            if (isEnabled)
            {
                builder.UseWebMarkupMin();
            }

            return builder;
        }
    }
}