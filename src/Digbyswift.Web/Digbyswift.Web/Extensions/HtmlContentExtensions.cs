using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

namespace Digbyswift.Web.Extensions
{
    public static class HtmlContentExtensions
    {
        public static string GetString(this IHtmlContent content)
        {
            using (var writer = new StringWriter())
            {        
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            } 
        }     
    }
}
