using System;
using System.Net;
#if NETSTANDARD2_1        
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
#else
using System.Web.Mvc;
#endif
using Digbyswift.Web.Extensions;

namespace Digbyswift.Web.Mvc.Attributes
{
    /// <summary>
    /// Validates that a request has come from the same
    /// domain. This can of course be spoofed but is a
    /// reasonable approach for most cases.
    /// </summary>
    public sealed class ValidateXhrRequestAttribute : ActionFilterAttribute
    {
        
#if NETSTANDARD2_1
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.IsAjaxRequest())
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.BadRequest);
                return;
            }
            
            var referrer = context.HttpContext.Request.GetReferrer();
            if (referrer == null)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.BadRequest);
                return;
            }

            if (!referrer.Host.Equals(context.HttpContext.Request.GetRootUri().Host, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
        }
#else
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
                return;
            }
            
            var referrer = filterContext.HttpContext.Request.UrlReferrer;
            if (referrer == null)
            {
                filterContext.Result = new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
                return;
            }

            if (referrer.Host.Equals(filterContext.HttpContext.Request.Url?.Host ?? String.Empty, StringComparison.OrdinalIgnoreCase))
            {
                filterContext.Result = new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
            }
        }
#endif
        
   }
}