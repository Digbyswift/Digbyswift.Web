using System;
using Digbyswift.Core.Constants;
using Digbyswift.Web.Constants;
#if NETSTANDARD2_1
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
#else
using Digbyswift.Core.RegularExpressions;
using System.Web;
#endif

namespace Digbyswift.Web.Extensions
{
    public static class HttpRequestExtensions
	{
#if NETSTANDARD2_1
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

            var newQueryString = QueryHelpers.ParseQuery(request.QueryString.Value);
            newQueryString[replaceKey] = value.ToString();

            var queryBuilder = new QueryBuilder(newQueryString
                .Where(x => !String.IsNullOrWhiteSpace(x.Value))
                .ToDictionary(x => x.Key, x => x.Value.ToString())
            );

            return new UriBuilder
            {            
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.Path.ToString(),
                Query = queryBuilder.ToQueryString().ToUriComponent()
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

            var newQueryString = QueryHelpers.ParseQuery(request.QueryString.Value);
            newQueryString.Remove(excludeKey);

            var queryBuilder = new QueryBuilder(newQueryString
                .Where(x => !String.IsNullOrWhiteSpace(x.Value))
                .ToDictionary(x => x.Key, x => x.Value.ToString())
            );

            return new UriBuilder
            {            
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.Path.ToString(),
                Query = queryBuilder.ToQueryString().ToUriComponent()
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
#else        
        public static HttpRequestBase AsBase(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            
            return new HttpRequestWrapper(request);
        }
        
        public static string RootUrl(this HttpRequest request)
        {
            return request.AsBase().RootUrl();
        }

        public static string RootUrl(this HttpRequestBase request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            
            return request.Url.BaseUrl();
        }
        
        public static string IpAddress(this HttpRequest request)
        {
            return request.AsBase().IpAddress();
        }

        public static string IpAddress(this HttpRequestBase request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            
            string ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!String.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return request.ServerVariables["REMOTE_ADDR"];
        }

        public static bool PathHasExtension(this HttpRequest request)
        {
            return request.AsBase().PathHasExtension();
        }
        
        public static bool PathHasExtension(this HttpRequestBase request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (String.IsNullOrWhiteSpace(request.Path))
                throw new ArgumentException("Request has no path");
            
            return RegexPatterns.FileExtensionRegex.IsMatch(request.Path);
        }        
        
        public static bool IsGetMethod(this HttpRequest request)
        {
            return request.AsBase().IsGetMethod();
        }

        public static bool IsGetMethod(this HttpRequestBase request)
        {
            return request.HttpMethod.Equals(HttpConstants.Methods.Get, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsPostMethod(this HttpRequest request)
        {
            return request.AsBase().IsPostMethod();
        }

        public static bool IsPostMethod(this HttpRequestBase request)
        {
            return request.HttpMethod.Equals(HttpConstants.Methods.Post, StringComparison.OrdinalIgnoreCase);
        }
        
        public static bool HasInternalReferrer(this HttpRequest request)
        {
            return request.AsBase().HasInternalReferrer();
        }

        public static bool HasInternalReferrer(this HttpRequestBase request)
        {
            return request.UrlReferrer?.ToString().StartsWith(request.RootUrl()) ?? false;
        }

        public static Uri UrlReferrerOrDefault(this HttpRequestBase request, string defaultReferrer = StringConstants.ForwardSlash)
        {
            if (request.UrlReferrer != null && request.HasInternalReferrer())
                return request.UrlReferrer;
            
            if (defaultReferrer.StartsWith(Uri.UriSchemeHttps) || defaultReferrer.StartsWith(Uri.UriSchemeHttp))
                return new Uri(defaultReferrer);

            var uriBuilder = new UriBuilder()
            {
                Scheme = request.Url.Scheme,
                Host = request.Url.Host,
                Path = defaultReferrer,
            };

            return uriBuilder.Uri;
        }

        public static string PathAndQueryWithoutKey(this HttpRequest request, string excludeKey)
        {
            return request.AsBase().PathAndQueryWithoutKey(excludeKey);
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

        public static string PathAndQueryReplaceKey(this HttpRequest request, string replaceKey, object value)
        {
            return request.AsBase().PathAndQueryReplaceKey(replaceKey, value);
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
#endif
        
    }
}