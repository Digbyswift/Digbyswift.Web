using System;
using System.Web;
using Digbyswift.Core.Constants;
using Digbyswift.Web.Constants;
using System.Net;
using Microsoft.AspNetCore.Http;
using Digbyswift.Extensions;

namespace Digbyswift.Web.Extensions
{
    public static class HttpRequestExtensions
	{
        public static Uri GetRootUri(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return new UriBuilder
            {            
                Scheme = request.Scheme,
                Host = request.Host.Host
            }.Uri;
        }

        public static string GetRootUrl(this HttpRequest request)
        {
            return GetRootUri(request).ToString().TrimEnd(CharConstants.ForwardSlash);
        }

        public static Uri GetAbsoluteUri(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return new UriBuilder
            {            
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString()
            }.Uri;
        }

        public static string GetAbsoluteUrl(this HttpRequest request)
        {
            return request.GetAbsoluteUri().ToString();
        }

        public static IPAddress GetIpAddress(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return request.HttpContext.Connection.RemoteIpAddress;
        }

        public static Uri ReplaceQueryKey(this HttpRequest request, string replaceKey, object value)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (replaceKey == null)
                throw new ArgumentNullException(nameof(replaceKey));

            if (!request.Query?.ContainsKey(replaceKey) ?? true)
                return request.GetAbsoluteUri();

            var newQueryString = HttpUtility.ParseQueryString(request.QueryString.Value);
            newQueryString[replaceKey] = value.ToString();

            return new UriBuilder
            {            
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.Path.ToString(),
                Query = newQueryString.ToQueryString()
            }.Uri;
        }
        
        public static Uri RemoveQueryKey(this HttpRequest request, string excludeKey)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (excludeKey == null)
                throw new ArgumentNullException(nameof(excludeKey));

            if (!request.Query?.ContainsKey(excludeKey) ?? true)
                return request.GetAbsoluteUri();

            // this gets all the query string key value pairs as a collection
            var newQueryString = HttpUtility.ParseQueryString(request.QueryString.Value);
            newQueryString.Remove(excludeKey);

            return new UriBuilder
            {            
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.Path.ToString(),
                Query = newQueryString.ToQueryString()
            }.Uri;
        }

        public static bool HasReferrer(this HttpRequest request)
        {
            return request.Headers.ContainsKey(HttpConstants.Headers.Referrer) && !String.IsNullOrWhiteSpace(request.Headers[HttpConstants.Headers.Referrer][0]);
        }
        
        public static bool HasInternalReferrer(this HttpRequest request)
        {
            return HasReferrer(request) && request.Headers[HttpConstants.Headers.Referrer][0].StartsWith(request.GetRootUrl());
        }
        
        public static Uri GetReferrer(this HttpRequest request)
        {
            return !request.HasReferrer() ? null : new Uri(request.Headers[HttpConstants.Headers.Referrer]);
        }
        
        public static Uri GetReferrerOrDefault(this HttpRequest request, string url = StringConstants.ForwardSlash)
        {
            return GetReferrer(request) ?? new Uri(url);
        }
        
        public static Uri GetReferrerOrDefault(this HttpRequest request, Uri uri)
        {
            return GetReferrer(request) ?? uri;
        }
        
        public static Uri GetSafeUrlReferrer(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!HasReferrer(request))
                return null;

            return HasInternalReferrer(request) ? GetReferrer(request) : null;
        }

        public static Uri GetSafeUrlReferrerOrDefault(this HttpRequest request, string defaultReferrer = StringConstants.ForwardSlash)
        {
            var referer = request.GetSafeUrlReferrer();
            if (referer != null)
                return referer;

            if (defaultReferrer.StartsWith(Uri.UriSchemeHttps) || defaultReferrer.StartsWith(Uri.UriSchemeHttp))
                return new Uri(defaultReferrer);
            
            return new UriBuilder()
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = defaultReferrer,
            }.Uri;
        }
        
        public static Uri GetSafeUrlReferrerOrDefault(this HttpRequest request, Uri defaultReferrer)
        {
            return request.GetSafeUrlReferrer() ?? defaultReferrer;
        }
        
        public static bool IsAjaxRequest(this HttpRequest request, string header = "X-Requested-With", string headerValue = HttpConstants.Headers.XmlHttpRequest)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!request.Headers.ContainsKey(header))
                return false;
            
            return request.Headers[header] == headerValue;
        }

        public static bool IsGetMethod(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return request.Method.Equals(HttpConstants.Methods.Get, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsHeadMethod(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return request.Method.Equals(HttpConstants.Methods.Head, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsPostMethod(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return request.Method.Equals(HttpConstants.Methods.Post, StringComparison.OrdinalIgnoreCase);
        }
       
	}
}