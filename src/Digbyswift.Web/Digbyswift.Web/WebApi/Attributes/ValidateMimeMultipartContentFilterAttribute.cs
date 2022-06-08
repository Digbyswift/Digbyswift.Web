#if NETSTANDARD2_1
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
#else
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
#endif

namespace Digbyswift.Web.WebApi.Attributes
{
#if NETSTANDARD2_1
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
#else
    public class ValidateMimeMultipartContentFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
        }
    }
#endif

}