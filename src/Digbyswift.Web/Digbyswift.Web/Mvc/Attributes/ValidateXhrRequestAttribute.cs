using System.Net;
using System.Web.Mvc;

namespace Digbyswift.Web.Mvc.Attributes
{
    /// <summary>
    /// Validates that a request has come from the same
    /// domain. This can of course be spoofed but is a
    /// reasonable approach for most cases.
    /// </summary>
    public sealed class ValidateXhrRequestAttribute : ActionFilterAttribute
    {
        private static readonly HttpStatusCodeResult BadRequestResult = new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = BadRequestResult;
                return;
            }
            
            var referrer = filterContext.HttpContext.Request.UrlReferrer;
            if (referrer == null)
            {
                filterContext.Result = BadRequestResult;
                return;
            }

            if (referrer.Host != filterContext.HttpContext.Request.Url?.Host)
            {
                filterContext.Result = BadRequestResult;
            }
        }
   }

}