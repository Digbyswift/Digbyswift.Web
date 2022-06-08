#if NET462
using System.Web;
using System.Web.Http.WebHost;

namespace Digbyswift.Web.WebApi
{
	// http://blogs.msdn.com/b/kiranchalla/archive/2012/09/04/receiving-request-file-or-data-in-streamed-mode-at-a-web-api-service.aspx
	public class NoBufferPolicySelector : WebHostBufferPolicySelector
	{
		public override bool UseBufferedInputStream(object hostContext)
		{
			var context = hostContext as HttpContextBase;

			if (context?.Request.ContentType != null && context.Request.ContentType.Contains("multipart"))
				return false;

			return base.UseBufferedInputStream(hostContext);
		}
	}
}
#endif