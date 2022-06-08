#if NETSTANDARD2_1        
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
#else
using System;
using System.Net;
using System.Web.Mvc;
#endif

namespace Digbyswift.Web.Mvc.Attributes
{
    public sealed class ImportModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.Controller as Controller;
            if (controller?.TempData[ExportModelStateAttribute.TempDataKey] is ModelStateDictionary modelState)
            {
                // Only Import if we are viewing
                if (context.Result is ViewResult)
                {
                    controller.ViewData.ModelState.Merge(modelState);
                }
                else
                {
                    controller.TempData.Remove(ExportModelStateAttribute.TempDataKey);
                }
            }

            base.OnActionExecuted(context);
        }
    }

    public class ExportModelStateAttribute : ActionFilterAttribute
    {
        public const string TempDataKey = "ExportModelStateToTempData";

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (!(context.Controller is Controller controller))
                return;
            
            if (!controller.ViewData.ModelState.IsValid && IsValidResult(context.Result))
            {
                controller.TempData[TempDataKey] = controller.ViewData.ModelState;
            }

            base.OnActionExecuted(context);
        }

#if NETSTANDARD2_1        
        public virtual bool IsValidResult(IActionResult result)
        {
            return result is RedirectResult ||
                   result is RedirectToRouteResult ||
                   result is JsonResult;
        }
#else
        public virtual bool IsValidResult(ActionResult result)
        {
            return result is RedirectResult ||
                   result is RedirectToRouteResult ||
                   result is JsonResult;
        }
#endif
    }
}
