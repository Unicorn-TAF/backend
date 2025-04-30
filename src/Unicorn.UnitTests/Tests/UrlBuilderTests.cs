using System.Collections.Generic;
using Unicorn.Backend.Utils;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.UnitTests.Tests
{
    [Suite]
    public class UrlBuilderTests
    {
        private const string BaseUrl = "/page.html";

        [Author("Vitalii Dobriian")]
        [Test]
        public void TestUrlBuilderHandlesEndpointWithLeadingSlash()
        {
            string path = new UrlBuilder("/").Append(BaseUrl).Build();
            Assert.That(path, Is.EqualTo(BaseUrl));
        }

        [Author("Vitalii Dobriian")]
        [Test]
        public void TestUrlBuilderHandlesEndpointWithTrailingSlash()
        {
            string path = new UrlBuilder("/base/").Append(BaseUrl).Build();
            Assert.That(path, Is.EqualTo($"/base{BaseUrl}"));
        }

        [Author("Vitalii Dobriian")]
        [Test]
        public void TestUrlBuilderHandlesEndpointWithQueryParametersAndFragmentIdentifier()
        {
            string path = new UrlBuilder("").Append(BaseUrl).Fragment("section1").Query("param", "value").Build();
            Assert.That(path, Is.EqualTo("/page.html#section1?param=value"));
        }

        [Author("Vitalii Dobriian")]
        [Test]
        public void TestUrlBuilderHandlesEndpointWithMultipleQueryParametersAndFragmentIdentifier()
        {
            string path = new UrlBuilder("").Append(BaseUrl)
                .Fragment("section2").Query("param1", "value1").Query("param2", "value2").Build();

            Assert.That(path, Is.EqualTo("/page.html#section2?param1=value1&param2=value2"));
        }

        [Author("Vitalii Dobriian")]
        [Test]
        public void TestUrlBuilderHandlesEndpointWithMultipleQueryParametersAsDictionary()
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                { "param1", "value1" },
                { "param2", "value2" }
            };

            string path = new UrlBuilder(BaseUrl)
                .Query(queryParams).Build();

            Assert.That(path, Is.EqualTo("/page.html?param1=value1&param2=value2"));
        }

        [Author("Vitalii Dobriian")]
        [Test]
        public void TestUrlBuilderHandlesEndpointWithArrayQueryParameters()
        {
            string path = new UrlBuilder("").Append(BaseUrl)
                .Query("param", new string[] { "value1", "value2" }).Build();

            Assert.That(path, Is.EqualTo("/page.html?param=value1&param=value2"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test]
        public void TestUrlBuilderReturnsCorrectPathForFileEndpoint()
        {
            string path = new UrlBuilder("https://base").Append(BaseUrl).Build();
            Assert.That(path, Is.EqualTo("https://base/page.html"));
        }
    }
}
