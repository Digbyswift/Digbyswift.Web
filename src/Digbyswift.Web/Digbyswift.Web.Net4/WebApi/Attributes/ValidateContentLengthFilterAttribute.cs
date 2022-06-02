#if NET461
using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Digbyswift.Web.Net4.WebApi.Attributes
{
	public class ValidateContentLengthFilterAttribute : ActionFilterAttribute
	{
		private readonly long _maxImageUploadBytes;

		public ValidateContentLengthFilterAttribute(long maxImageUploadBytes)
		{
			_maxImageUploadBytes = maxImageUploadBytes;
		}
		
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			if (actionContext.Request.Content.Headers.ContentLength > _maxImageUploadBytes)
			{
				throw new HttpResponseException(HttpStatusCode.RequestEntityTooLarge);
			}
		}
	}
}
#endif