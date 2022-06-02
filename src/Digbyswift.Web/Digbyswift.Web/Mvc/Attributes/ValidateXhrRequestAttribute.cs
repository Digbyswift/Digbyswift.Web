using System;
using System.Net;
using Digbyswift.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Digbyswift.Web.Mvc.Attributes
{
    /// <summary>
    /// Validates that a request has come from the same
    /// domain. This can of course be spoofed but is a
    /// reasonable approach for most cases.
    /// </summary>
    public sealed class ValidateXhrRequestAttribute : ActionFilterAttribute
    {
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
   }
}