using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Web;
using Moq;
using NUnit.Framework;
using Serilog.Enrichers;
using Serilog.Events;
using Serilog.Providers;
using Serilog.Tests.Support;

namespace Serilog.Tests.Enrichers
{
    [TestFixture]
    [Parallelizable]
    public class RequestEnricherTests
    {
        [SetUp]
        public void SetUp()
        {
            _httpRequestProvider = new Mock<IHttpRequestProvider>();
            _httpRequestWrapper = new Mock<IHttpRequestWrapper>();
            _logger = new LoggerConfiguration()
                .Enrich.With(new RequestEnricher(_httpRequestProvider.Object))
                .WriteTo.Sink(new DelegatingSink(e => _logEvent = e))
                .CreateLogger();

            _httpRequestProvider.Setup(x => x.GetRequest()).Returns(_httpRequestWrapper.Object);
        }

        private Mock<IHttpRequestProvider> _httpRequestProvider;
        private Mock<IHttpRequestWrapper> _httpRequestWrapper;
        private ILogger _logger;
        private LogEvent _logEvent;

        private static Mock<IHttpPostedFileWrapper> CreateMockHttpPostedFile(
            string filename, 
            int contentLength,
            string contentType)
        {
            var mock = new Mock<IHttpPostedFileWrapper>();

            mock.SetupGet(x => x.FileName).Returns(filename);
            mock.SetupGet(x => x.ContentLength).Returns(contentLength);
            mock.SetupGet(x => x.ContentType).Returns(contentType);

            return mock;
        }

        [Test]
        public void ShouldCreateRequestAcceptTypesProperty()
        {
            _httpRequestWrapper.SetupGet(x => x.AcceptTypes).Returns(new[] {"Type1", "Type2"});

            _logger.Information(@"Has a Request.AcceptTypes property");

            Assert.NotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.AcceptTypes"));
            Assert.AreEqual("[\"Type1\", \"Type2\"]", _logEvent.Properties["Request.AcceptTypes"].LiteralValue());
        }

        [Test]
        public void ShouldCreateRequestAnonymousIDProperty()
        {
            _httpRequestWrapper.SetupGet(x => x.AnonymousID).Returns("SET");

            _logger.Information(@"Has a Request.AnonymousID property");

            Assert.NotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.AnonymousID"));
            Assert.AreEqual("\"SET\"", _logEvent.Properties["Request.AnonymousID"].LiteralValue());
        }
        
        [Test]
        public void ShouldCreateRequestApplicationPathProperty()
        {
            _httpRequestWrapper.SetupGet(x => x.ApplicationPath).Returns("SET");

            _logger.Information(@"Has a Request.ApplicationPath property");

            Assert.NotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.ApplicationPath"));
            Assert.AreEqual("\"SET\"", _logEvent.Properties["Request.ApplicationPath"].LiteralValue());
        }

        [Test]
        public void ShouldCreateRequestContentEncodingProperty()
        {
            _httpRequestWrapper.SetupGet(x => x.ContentEncoding).Returns(Encoding.UTF32);

            _logger.Information(@"Has a Request.ContentEncoding property");

            Assert.NotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.ContentEncoding"));
            Assert.AreEqual("System.Text.UTF32Encoding", _logEvent.Properties["Request.ContentEncoding"].LiteralValue());
        }
        
        [Test]
        public void ShouldCreateRequestContentLengthProperty()
        {
            _httpRequestWrapper.SetupGet(x => x.ContentLength).Returns(123);

            _logger.Information(@"Has a Request.ContentLength property");

            Assert.NotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.ContentLength"));
            Assert.AreEqual("123", _logEvent.Properties["Request.ContentLength"].LiteralValue());
        }
        
        [Test]
        public void ShouldCreateRequestContentTypeProperty()
        {
            _httpRequestWrapper.SetupGet(x => x.ContentType).Returns("SET");

            _logger.Information(@"Has a Request.ContentType property");

            Assert.NotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.ContentType"));
            Assert.AreEqual("\"SET\"", _logEvent.Properties["Request.ContentType"].LiteralValue());
        }
        
        [Test]
        public void ShouldCreateRequestHttpMethodProperty()
        {
            _httpRequestWrapper.SetupGet(x => x.HttpMethod).Returns("GET");

            _logger.Information(@"Has a Request.HttpMethod property");

            Assert.NotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.HttpMethod"));
            Assert.AreEqual("\"GET\"", _logEvent.Properties["Request.HttpMethod"].LiteralValue());
        }
        
        [Test]
        public void ShouldCreateRequestIsAuthenticatedProperty()
        {
            _httpRequestWrapper.SetupGet(x => x.IsAuthenticated).Returns(true);

            _logger.Information(@"Has a Request.IsAuthenticated property");

            Assert.NotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.IsAuthenticated"));
            Assert.AreEqual("True", _logEvent.Properties["Request.IsAuthenticated"].LiteralValue());
        }
        
        [Test]
        public void ShouldCreateRequestIsLocalProperty()
        {
            _httpRequestWrapper.SetupGet(x => x.IsLocal).Returns(true);

            _logger.Information(@"Has a Request.IsLocal property");

            Assert.NotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.IsLocal"));
            Assert.AreEqual("True", _logEvent.Properties["Request.IsLocal"].LiteralValue());
        }
        
        [Test]
        public void ShouldCreateRequestIsSecureConnectionProperty()
        {
            _httpRequestWrapper.SetupGet(x => x.IsSecureConnection).Returns(true);

            _logger.Information(@"Has a Request.IsSecureConnection property");

            Assert.NotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.IsSecureConnection"));
            Assert.AreEqual("True", _logEvent.Properties["Request.IsSecureConnection"].LiteralValue());
        }
        
        [Test]
        public void ShouldCreateRequestPhysicalApplicationPathProperty()
        {
            _httpRequestWrapper.SetupGet(x => x.PhysicalApplicationPath).Returns("SET");

            _logger.Information(@"Has a Request.PhysicalApplicationPath property");

            Assert.NotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.PhysicalApplicationPath"));
            Assert.AreEqual("\"SET\"", _logEvent.Properties["Request.PhysicalApplicationPath"].LiteralValue());
        }

        [Test]
        public void ShouldCreateRequestPhysicalPathProperty()
        {
            _httpRequestWrapper.SetupGet(x => x.PhysicalPath).Returns("SET");

            _logger.Information(@"Has a Request.PhysicalPath property");

            Assert.NotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.PhysicalPath"));
            Assert.AreEqual("\"SET\"", _logEvent.Properties["Request.PhysicalPath"].LiteralValue());
        }
        
        [Test]
        public void ShouldCreateRequestRawUrlProperty()
        {
            _httpRequestWrapper.SetupGet(x => x.RawUrl).Returns("SET");

            _logger.Information(@"Has a Request.RawUrl property");

            Assert.NotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.RawUrl"));
            Assert.AreEqual("\"SET\"", _logEvent.Properties["Request.RawUrl"].LiteralValue());
        }

        [Test]
        public void ShouldCreateRequestRequestTypeProperty()
        {
            _httpRequestWrapper.SetupGet(x => x.RequestType).Returns("SET");

            _logger.Information(@"Has a Request.RequestType property");

            Assert.NotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.RequestType"));
            Assert.AreEqual("\"SET\"", _logEvent.Properties["Request.RequestType"].LiteralValue());
        }

        [Test]
        public void ShouldCreateRequestTotalBytesProperty()
        {
            _httpRequestWrapper.SetupGet(x => x.TotalBytes).Returns(100);

            _logger.Information(@"Has a Request.TotalBytes property");

            Assert.NotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.TotalBytes"));
            Assert.AreEqual("100", _logEvent.Properties["Request.TotalBytes"].LiteralValue());
        }
        
        [Test]
        public void ShouldCreateRequestUrlProperty()
        {
            _httpRequestWrapper.SetupGet(x => x.Url).Returns(new Uri("http://serilog.net/"));

            _logger.Information(@"Has a Request.Url property");

            Assert.NotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.Url"));
            Assert.AreEqual("\"http://serilog.net/\"", _logEvent.Properties["Request.Url"].LiteralValue());
        }
        
        [Test]
        public void ShouldCreateRequestUrlReferrerProperty()
        {
            _httpRequestWrapper.SetupGet(x => x.UrlReferrer).Returns(new Uri("http://serilog.net/"));

            _logger.Information(@"Has a Request.UrlReferrer property");

            Assert.NotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.UrlReferrer"));
            Assert.AreEqual("\"http://serilog.net/\"", _logEvent.Properties["Request.UrlReferrer"].LiteralValue());
        }

        [Test]
        public void ShouldCreateRequestUserAgentProperty()
        {
            _httpRequestWrapper.SetupGet(x => x.UserAgent).Returns("SET");

            _logger.Information(@"Has a Request.UserAgent property");

            Assert.NotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.UserAgent"));
            Assert.AreEqual("\"SET\"", _logEvent.Properties["Request.UserAgent"].LiteralValue());
        }
        
        [Test]
        public void ShouldCreateRequestUserHostAddressProperty()
        {
            _httpRequestWrapper.SetupGet(x => x.UserHostAddress).Returns("SET");

            _logger.Information(@"Has a Request.UserHostAddress property");

            Assert.NotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.UserHostAddress"));
            Assert.AreEqual("\"SET\"", _logEvent.Properties["Request.UserHostAddress"].LiteralValue());
        }
        
        [Test]
        public void ShouldCreateRequestUserHostNameProperty()
        {
            _httpRequestWrapper.SetupGet(x => x.UserHostName).Returns("SET");

            _logger.Information(@"Has a Request.UserHostName property");

            Assert.NotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.UserHostName"));
            Assert.AreEqual("\"SET\"", _logEvent.Properties["Request.UserHostName"].LiteralValue());
        }

        [Test]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public void ShouldCreateRequestCookiesProperties()
        {
            var cookies = new HttpCookieCollection
            {
                new HttpCookie("test-1", "My First Cookie!"),
                new HttpCookie("test-2", "My Second Cookie!"),
                new HttpCookie("test-3", "My Third Cookie!")
            };

            _httpRequestWrapper.SetupGet(x => x.Cookies).Returns(cookies);

            _logger.Information(@"Has Request.Cookies properties");

            Assert.IsNotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.Cookies[test-1].Name"));
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.Cookies[test-2].Name"));
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.Cookies[test-3].Name"));
            Assert.AreEqual(cookies["test-1"].Name,
                _logEvent.Properties["Request.Cookies[test-1].Name"].LiteralValue());
            Assert.AreEqual(cookies["test-1"].Value,
                _logEvent.Properties["Request.Cookies[test-1].Value"].LiteralValue());
            Assert.AreEqual(cookies["test-1"].Domain,
                _logEvent.Properties["Request.Cookies[test-1].Domain"].LiteralValue());
            Assert.AreEqual(cookies["test-1"].Expires.ToString("u"),
                _logEvent.Properties["Request.Cookies[test-1].Expires"].LiteralValue());
            Assert.AreEqual(cookies["test-1"].Path,
                _logEvent.Properties["Request.Cookies[test-1].Path"].LiteralValue());
            Assert.AreEqual(cookies["test-2"].Name,
                _logEvent.Properties["Request.Cookies[test-2].Name"].LiteralValue());
            Assert.AreEqual(cookies["test-2"].Value,
                _logEvent.Properties["Request.Cookies[test-2].Value"].LiteralValue());
            Assert.AreEqual(cookies["test-2"].Domain,
                _logEvent.Properties["Request.Cookies[test-2].Domain"].LiteralValue());
            Assert.AreEqual(cookies["test-2"].Expires.ToString("u"),
                _logEvent.Properties["Request.Cookies[test-2].Expires"].LiteralValue());
            Assert.AreEqual(cookies["test-2"].Path,
                _logEvent.Properties["Request.Cookies[test-2].Path"].LiteralValue());
            Assert.AreEqual(cookies["test-3"].Name,
                _logEvent.Properties["Request.Cookies[test-3].Name"].LiteralValue());
            Assert.AreEqual(cookies["test-3"].Value,
                _logEvent.Properties["Request.Cookies[test-3].Value"].LiteralValue());
            Assert.AreEqual(cookies["test-3"].Domain,
                _logEvent.Properties["Request.Cookies[test-3].Domain"].LiteralValue());
            Assert.AreEqual(cookies["test-3"].Expires.ToString("u"),
                _logEvent.Properties["Request.Cookies[test-3].Expires"].LiteralValue());
            Assert.AreEqual(cookies["test-3"].Path,
                _logEvent.Properties["Request.Cookies[test-3].Path"].LiteralValue());
        }

        [Test]
        public void ShouldCreateRequestFilesProperties()
        {
            var collection = new Mock<IHttpFileCollectionWrapper>();
            var files = new[]
            {
                CreateMockHttpPostedFile("test1.aspx", 1, "content-type-1"),
                CreateMockHttpPostedFile("test2.aspx", 2, "content-type-2"),
                CreateMockHttpPostedFile("test3.aspx", 3, "content-type-3")
            };

            _httpRequestWrapper.SetupGet(x => x.Files).Returns(collection.Object);
            collection.SetupGet(x => x.AllKeys).Returns(files.Select(x => x.Object.FileName).ToArray());
            collection
                .Setup(x => x.Get(It.IsAny<string>()))
                .Returns((string key) => files.Single(x => x.Object.FileName == key).Object);

            _logger.Information(@"Has Request.Files properties");

            Assert.IsNotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.Files[test1.aspx].FileName"));
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.Files[test2.aspx].FileName"));
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.Files[test3.aspx].FileName"));
            Assert.AreEqual("test1.aspx", _logEvent.Properties["Request.Files[test1.aspx].FileName"].LiteralValue());
            Assert.AreEqual(1, _logEvent.Properties["Request.Files[test1.aspx].ContentLength"].LiteralValue());
            Assert.AreEqual("content-type-1", _logEvent.Properties["Request.Files[test1.aspx].ContentType"].LiteralValue());
            Assert.AreEqual("test2.aspx", _logEvent.Properties["Request.Files[test2.aspx].FileName"].LiteralValue());
            Assert.AreEqual(2, _logEvent.Properties["Request.Files[test2.aspx].ContentLength"].LiteralValue());
            Assert.AreEqual("content-type-2", _logEvent.Properties["Request.Files[test2.aspx].ContentType"].LiteralValue());
            Assert.AreEqual("test3.aspx", _logEvent.Properties["Request.Files[test3.aspx].FileName"].LiteralValue());
            Assert.AreEqual(3, _logEvent.Properties["Request.Files[test3.aspx].ContentLength"].LiteralValue());
            Assert.AreEqual("content-type-3", _logEvent.Properties["Request.Files[test3.aspx].ContentType"].LiteralValue());
        }

        [Test]
        public void ShouldCreateRequestFormProperties()
        {
            var form = new NameValueCollection
            {
                {"username", "john_smith"},
                {"password", "mega-secure-password"}
            };

            _httpRequestWrapper.SetupGet(x => x.Form).Returns(form);

            _logger.Information(@"Has Request.Form properties");

            Assert.IsNotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.Form[username]"));
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.Form[password]"));
            Assert.AreEqual(form["username"], _logEvent.Properties["Request.Form[username]"].LiteralValue());
            Assert.AreEqual(form["password"], _logEvent.Properties["Request.Form[password]"].LiteralValue());
        }

        [Test]
        public void ShouldCreateRequestHeadersProperties()
        {
            var headers = new NameValueCollection
            {
                {"Accept", "application/json"},
                {"Content-Type", "application/json"},
                {"User-Agent", "NUnit-Test"}
            };

            _httpRequestWrapper.SetupGet(x => x.Headers).Returns(headers);

            _logger.Information(@"Has Request.Headers properties");

            Assert.IsNotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.Headers[Accept]"));
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.Headers[Content-Type]"));
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.Headers[User-Agent]"));
            Assert.AreEqual(headers["Accept"], _logEvent.Properties["Request.Headers[Accept]"].LiteralValue());
            Assert.AreEqual(headers["Content-Type"], _logEvent.Properties["Request.Headers[Content-Type]"].LiteralValue());
            Assert.AreEqual(headers["User-Agent"], _logEvent.Properties["Request.Headers[User-Agent]"].LiteralValue());
        }

        [Test]
        public void ShouldCreateRequestParamsProperties()
        {
            var parameters = new NameValueCollection
            {
                {"username", "john_smith"},
                {"password", "mega-secure-password"}
            };

            _httpRequestWrapper.SetupGet(x => x.Params).Returns(parameters);

            _logger.Information(@"Has Request.Params properties");

            Assert.IsNotNull(_logEvent);
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.Params[username]"));
            Assert.IsTrue(_logEvent.Properties.ContainsKey("Request.Params[password]"));
            Assert.AreEqual(parameters["username"], _logEvent.Properties["Request.Params[username]"].LiteralValue());
            Assert.AreEqual(parameters["password"], _logEvent.Properties["Request.Params[password]"].LiteralValue());
        }
    }
}