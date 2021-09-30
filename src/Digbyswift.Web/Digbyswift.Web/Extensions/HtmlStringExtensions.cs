using System;
using System.Web;
using System.Web.Mvc;

namespace Digbyswift.Web.Extensions
{
    public static class HtmlStringExtensions
    {
        public static IHtmlString AsRichText(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return null;

            return new HtmlString(value.Replace("\n", "<br />"));
        }

        public static bool IsNullOrWhiteSpace(IHtmlString instance)
        {
            if (instance == null)
            {
                return true;
            }

            if (instance is MvcHtmlString mvcHtmlString)
            {
                return string.IsNullOrWhiteSpace(mvcHtmlString.ToHtmlString());
            }

            if (instance is HtmlString htmlString)
            {
                return string.IsNullOrWhiteSpace(htmlString.ToHtmlString());
            }

            throw new NotImplementedException($"{instance.GetType().FullName} not supported.");
        }

        public static bool IsNullOrEmpty(IHtmlString instance)
        {
            if (instance == null)
            {
                return true;
            }

            if (instance is MvcHtmlString mvcHtmlString)
            {
                return string.IsNullOrWhiteSpace(mvcHtmlString.ToHtmlString());
            }

            if (instance is HtmlString htmlString)
            {
                return string.IsNullOrWhiteSpace(htmlString.ToHtmlString());
            }

            throw new NotImplementedException($"{instance.GetType().FullName} not supported.");
        }
    }
}