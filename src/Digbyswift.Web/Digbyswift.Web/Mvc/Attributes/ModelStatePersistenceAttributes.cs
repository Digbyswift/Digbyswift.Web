using System.Web.Mvc;

namespace Digbyswift.Web.Mvc.Attributes
{
    public sealed class ImportModelState : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Controller.TempData[ExportModelState.TempDataKey] is ModelStateDictionary modelState)
            {
                // Only Import if we are viewing
                if (filterContext.Result is ViewResult)
                {
                    filterContext.Controller.ViewData.ModelState.Merge(modelState);
                }
                else
                {
                    filterContext.Controller.TempData.Remove(ExportModelState.TempDataKey);
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }

    public class ExportModelState : ActionFilterAttribute
    {
        public const string TempDataKey = "ExportModelStateToTempData";

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Controller.ViewData.ModelState.IsValid == false)
            {
                if (IsValidResult(filterContext.Result))
                {
                    filterContext.Controller.TempData[TempDataKey] = filterContext.Controller.ViewData.ModelState;
                }
            }

            base.OnActionExecuted(filterContext);
        }

        public virtual bool IsValidResult(ActionResult result)
        {
            return result is RedirectResult ||
                   result is RedirectToRouteResult ||
                   result is JsonResult;
        }

    }
}