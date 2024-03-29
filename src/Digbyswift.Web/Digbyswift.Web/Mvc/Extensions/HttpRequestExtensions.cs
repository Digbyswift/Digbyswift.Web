﻿using System;
using System.Web;
using Digbyswift.Core.Constants;
using Digbyswift.Core.RegularExpressions;
using Digbyswift.Web.Extensions;

namespace Digbyswift.Web.Mvc.Extensions
{
    public static class HttpMethod
    {
        public const string Get = "Get";
        public const string Post = "Post";
    }

	public static class HttpRequestExtensions
	{
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
            return request.Url?.BaseUrl();
        }
        
        public static string IpAddress(this HttpRequest request)
        {
            return request.AsBase().IpAddress();
        }

        public static string IpAddress(this HttpRequestBase request)
        {
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

        public static bool IsInternalReferrer(this HttpRequest request)
        {
            return request.AsBase().IsInternalReferrer();
        }

        public static bool IsInternalReferrer(this HttpRequestBase request)
        {
            return request.UrlReferrer?.ToString().StartsWith(request.RootUrl()) ?? false;
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
            return request.HttpMethod.Equals(HttpMethod.Get, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsPostMethod(this HttpRequest request)
        {
            return request.AsBase().IsPostMethod();
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

	}
}