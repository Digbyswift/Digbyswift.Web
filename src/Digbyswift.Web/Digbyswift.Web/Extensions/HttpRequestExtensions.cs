using System;
using System.Web;
using Digbyswift.Core.Constants;

namespace Digbyswift.Web.Extensions
{
    public static class HttpMethod
    {
        public const string Get = "Get";
        public const string Post = "Post";
    }

	public static class HttpRequestExtensions
	{
        public static bool IsGetMethod(this HttpRequestBase request)
        {
            return request.HttpMethod.Equals(HttpMethod.Get, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsPostMethod(this HttpRequestBase request)
        {
            return request.HttpMethod.Equals(HttpMethod.Post, StringComparison.OrdinalIgnoreCase);
        }

        public static Uri UrlReferrerOrDefault(this HttpRequestBase request, string defaultReferrer = StringConstants.ForwardSlash)
        {
            if (request.UrlReferrer != null && request.UrlReferrer.ToString().IsInternalUrl())
                return request.UrlReferrer;

            var uriBuilder = new UriBuilder()
            {
                Path = defaultReferrer,
                Scheme = request.Url.Scheme,
                Host = request.Url.Host
            };

            return uriBuilder.Uri;
        }
        
        public static string BaseUrl(this HttpRequestBase request)
        {
            return request.Url?.GetLeftPart(UriPartial.Authority);
        }

        public static string BaseUrl(this Uri uri)
        {
            return uri.GetLeftPart(UriPartial.Authority);
        }

        public static string AbsoluteUrlPathOnly(this Uri uri)
        {
            return $"{uri.GetLeftPart(UriPartial.Authority)}{uri.AbsolutePath}";
        }

        public static string PathAndQueryWithoutKey(this HttpRequestBase request, string excludeKey)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var uri = request.Url;

            // this gets all the query string key value pairs as a collection
            var newQueryString = HttpUtility.ParseQueryString(uri.Query);

            // this removes the key if exists
            newQueryString.Remove(excludeKey);

            // this gets the page path from root without QueryString
            string pagePathWithoutQueryString = uri.GetLeftPart(UriPartial.Path);

            return newQueryString.Count > 0
                ? $"{pagePathWithoutQueryString}?{newQueryString}"
                : pagePathWithoutQueryString;
        }

        public static string PathAndQueryReplaceKey(this HttpRequestBase request, string replaceKey, object value)
        {
            if(request == null)
                throw new ArgumentNullException(nameof(request));

            var uri = request.Url;

            // this gets all the query string key value pairs as a collection
            var newQueryString = HttpUtility.ParseQueryString(uri.Query);

            // this removes the key if exists
            newQueryString.Remove(replaceKey);

            // this gets the page path from root without QueryString
            string pagePathWithoutQueryString = uri.GetLeftPart(UriPartial.Path);

            return newQueryString.Count > 0
                ? $"{pagePathWithoutQueryString}?{replaceKey}={value}&{newQueryString}"
                : $"{pagePathWithoutQueryString}?{replaceKey}={value}";
        }

        public static string GetIpAddress(this HttpRequestBase request)
        {
            return request.UserHostAddress;
        }

	}
}