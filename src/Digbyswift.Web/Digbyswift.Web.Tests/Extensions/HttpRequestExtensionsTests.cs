using System;
using System.Net;
using Digbyswift.Web.Extensions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NUnit.Framework;

namespace Digbyswift.Web.Tests.Extensions
{
    [TestFixture]
    public class HttpRequestExtensionsTests
    {
        private const string Host = "www.digbyswift.com";
        private const string HostUrl = "https://www.digbyswift.com";
        private const string HostIp = "234.0.0.1";
        
        #region GetRootUrl

        [Test]
        public void GetRootUrl_Throws_WhenRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((HttpRequest)null).GetRootUrl());
        }

        [Test]
        public void GetRootUrl_ReturnsAbsoluteUrl_WhenRequestIsNotNull()
        {
            // Arrange
            var request = Substitute.For<HttpRequest>();
            request.Host.Returns(new HostString(Host));
            request.Scheme.Returns("https");

            // Act
            var result = request.GetRootUrl();
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
        public void GetRootUrl_ReturnsHostUrl_WhenRequestIsNotNull(string path)
        {
            // Arrange
            var request = Substitute.For<HttpRequest>();
            request.Host.Returns(new HostString(Host));
            request.Scheme.Returns("https");

            // Act
            var result = request.GetRootUrl();
        
            // Assert
            Assert.That(result, Is.EqualTo(HostUrl));
        }

        #endregion

        #region GetRootUri

        [Test]
        public void GetRootUri_Throws_WhenRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((HttpRequest)null).GetRootUri());
        }

        [Test]
        public void GetRootUri_ReturnsAbsoluteUrl_WhenRequestIsNotNull()
        {
            // Arrange
            var request = Substitute.For<HttpRequest>();
            request.Host.Returns(new HostString(Host));
            request.Scheme.Returns("https");

            // Act
            var result = request.GetRootUri();
        
            // Assert
            Assert.That(result.IsAbsoluteUri, Is.True);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("/")]
        [TestCase("/robots.txt")]
        [TestCase("/about-us/contact")]
        [TestCase("/about-us/contact/?test-param=true")]
        public void GetRootUri_ReturnsHostUri_WhenRequestIsNotNull(string path)
        {
            // Arrange
            var request = Substitute.For<HttpRequest>();
            request.Host.Returns(new HostString(Host));
            request.Scheme.Returns("https");
            request.Path.Returns(new PathString(path));

            // Act
            var result = request.GetRootUri();
        
            // Assert
            Assert.That(result, Is.EqualTo(new Uri(HostUrl)));
        }

        #endregion

        #region GetIpAddress

        [Test]
        public void GetIpAddress_Throws_WhenRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((HttpRequest)null).GetIpAddress());
        }

        [Test]
        public void GetIpAddress_Returns()
        {
            // Arrange
            var ipAddress = IPAddress.Parse(HostIp);
            var request = Substitute.For<HttpRequest>();
            request.HttpContext.Connection.RemoteIpAddress.Returns(ipAddress);
            
            // Act
            var result = request.GetIpAddress();
        
            // Assert
            Assert.That(result, Is.EqualTo(ipAddress));
        }

        #endregion

    }
}