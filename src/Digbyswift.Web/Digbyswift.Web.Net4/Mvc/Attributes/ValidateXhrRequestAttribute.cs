using System.Web.Mvc;
using System;
using System.Net;

namespace Digbyswift.Web.Net4.Mvc.Attributes
{
    /// <summary>
    /// Validates that a request has come from the same
    /// domain. This can of course be spoofed but is a
    /// reasonable approach for most cases.
    /// </summary>
    public sealed class ValidateXhrRequestAttribute : ActionFilterAttribute
    {
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
    }
}