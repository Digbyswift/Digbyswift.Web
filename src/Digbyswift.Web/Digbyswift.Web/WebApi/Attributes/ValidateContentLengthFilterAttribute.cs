#if NETSTANDARD2_1
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
#else
using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
#endif

namespace Digbyswift.Web.WebApi.Attributes
{
	public class ValidateContentLengthAttribute : ActionFilterAttribute
	{
		private readonly long _maxImageUploadBytes;

		public ValidateContentLengthAttribute(long maxImageUploadBytes)
		{
			_maxImageUploadBytes = maxImageUploadBytes;
		}
		
#if NETSTANDARD2_1
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (context.HttpContext.Request.Headers.ContentLength > _maxImageUploadBytes)
			{
				context.Result = new StatusCodeResult((int)HttpStatusCode.RequestEntityTooLarge);
			}
		}
#else
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			if (actionContext.Request.Content.Headers.ContentLength > _maxImageUploadBytes)
			{
				throw new HttpResponseException(HttpStatusCode.RequestEntityTooLarge);
			}
		}
#endif
	}
}
