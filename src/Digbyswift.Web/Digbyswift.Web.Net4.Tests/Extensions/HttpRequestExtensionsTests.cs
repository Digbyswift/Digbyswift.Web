using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using Digbyswift.Web.Net4.Extensions;
using NSubstitute;
using NUnit.Framework;

namespace Digbyswift.Web.Tests.Extensions
{
    [TestFixture]
    public class HttpRequestExtensionsTests
    {
        private const string HostUrl = "https://www.digbyswift.com";
        private const string HostIp = "234.0.0.1";
        
        #region AsBase

        [Test]
        public void AsBase_Throws_WhenRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => HttpRequestExtensions.AsBase(null));
        }

        [Test]
        public void AsBase_ReturnsHttpRequestBase_WhenRequestIsNotNull()
        {
            using (var responseWriter = TextWriter.Null)
            {
                // Arrange
                HttpContext.Current = new HttpContext(
                    new HttpRequest(null, HostUrl, null),
                    new HttpResponse(responseWriter)
                );

                // Act
                var result = HttpContext.Current.Request.AsBase();
            
                // Assert
                Assert.That(result, Is.AssignableTo<HttpRequestBase>());
            }
        }

        #endregion
        
        #region RootUrl

        [Test]
        public void RootUrl_Throws_WhenRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((HttpRequest)null).RootUrl());
        }

        [Test]
        public void RootUrl_Throws_WhenRequestBaseIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((HttpRequestBase)null).RootUrl());
        }

        [Test]
        public void RootUrl_ReturnsAbsoluteUrl_WhenRequestIsNotNull()
        {
            using (var responseWriter = TextWriter.Null)
            {
                // Arrange
                HttpContext.Current = new HttpContext(
                    new HttpRequest(null, HostUrl, null),
                    new HttpResponse(responseWriter)
                );

                // Act
                var result = HttpContext.Current.Request.RootUrl();
                var resultUri = new Uri(result);
            
                // Assert
                Assert.That(resultUri.IsAbsoluteUri, Is.True);
            }
        }

        [Test]
        public void RootUrl_ReturnsAbsoluteUrl_WhenRequestBaseIsNotNull()
        {
            // Arrange
            var request = Substitute.For<HttpRequestBase>();
            request.Url.Returns(new Uri(HostUrl));
            
            // Act
            var result = request.RootUrl();
            var resultUri = new Uri(result);
            
            // Assert
            Assert.That(resultUri.IsAbsoluteUri, Is.True);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("/")]
        [TestCase("/robots.txt")]
        [TestCase("/about-us/contact")]
        [TestCase("/about-us/contact/?test-param=true")]
        public void RootUrl_ReturnsHostUrl_WhenRequestIsNotNull(string path)
        {
            using (var responseWriter = TextWriter.Null)
            {
                // Arrange
                HttpContext.Current = new HttpContext(
                    new HttpRequest(null, String.Concat(HostUrl, path), null),
                    new HttpResponse(responseWriter)
                );

                // Act
                var result = HttpContext.Current.Request.RootUrl();
            
                // Assert
                Assert.That(result, Is.EqualTo(HostUrl));
            }
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("/")]
        [TestCase("/robots.txt")]
        [TestCase("/about-us/contact")]
        [TestCase("/about-us/contact/?test-param=true")]
        public void RootUrl_ReturnsHostUrl_WhenRequestBaseIsNotNull(string path)
        {
            // Arrange
            var request = Substitute.For<HttpRequestBase>();
            request.Url.Returns(new Uri(HostUrl));
            
            // Act
            var result = request.RootUrl();

            // Assert
            Assert.That(result, Is.EqualTo(HostUrl));
        }

        #endregion

        #region IpAddress

        [Test]
        public void IpAddress_Throws_WhenRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((HttpRequest)null).IpAddress());
        }

        [Test]
        public void IpAddress_Throws_WhenRequestBaseIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((HttpRequestBase)null).IpAddress());
        }

        [Test]
        public void IpAddress_Returns()
        {
            // Arrange
            var request = Substitute.For<HttpRequestBase>();
            
            request.ServerVariables.Returns(new NameValueCollection{
                { "REMOTE_ADDR", HostIp },
                { "HTTP_X_FORWARDED_FOR", HostIp }
            });
            
            // Act
            var result = request.IpAddress();
        
            // Assert
            Assert.That(result, Is.EqualTo(HostIp));
        }

        #endregion

    }
}