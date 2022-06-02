using System;
using Digbyswift.Web.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Digbyswift.Web.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.SetNoCacheHeaders();

            base.OnResultExecuting(context);
        }
    }
}