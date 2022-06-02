using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Digbyswift.Web.WebApi.Attributes
{
    public class ValidateAsMultipartContentAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.GetMultipartBoundary() == null)
            {
                context.Result = new UnsupportedMediaTypeResult();
            }
        }
    }
}