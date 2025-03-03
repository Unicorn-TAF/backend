using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Unicorn.Backend.Services.RestService;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UnitTests.Dto;

namespace Unicorn.UnitTests.Tests
{
    [Suite]
    public class RestClientTests
    {
        private const string FileEndpoint = "/assets/" + ExpectedFileName;
        private const string ExpectedFileName = "test-file.txt";
        private static RestClient client;

        [BeforeSuite]
        public static void SetUp() =>
            client = new RestClient(Paths.ReastApiBaseUrl);

        [AfterSuite]
        public static void TearDown() =>
            client = null;

        [Author("Vitaliy Dobriyan")]
        [Test("Rest client sends correct GET request")]
        public void TestRestClientCorrectGetRequest()
        {
            RestResponse employee = client.SendRequest(HttpMethod.Get, "/objects/1");

            Assert.That(employee.Status, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(employee.Content, Text.Contains("Google Pixel 6 Pro"));
            Assert.That(employee.ExecutionTime, Is.IsLessThan(TimeSpan.FromSeconds(5)));

            Assert.IsTrue(employee.Headers.GetValues("server").Any(h => h.Contains("cloudflare")),
                "response does not contain expected header");
        }

        [Author("Vitaliy Dobriyan")]
        [Test("Rest client sends correct POST request")]
        public void TestRestClientCorrectPostRequest()
        {
            RestResponse response = client
                .SendRequest(HttpMethod.Post, "/objects", @"{""name"": ""Unicorn"",""data"": ""test json""}");

            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content, Text.Contains(@"""name"":""Unicorn"""));
        }

        [Author("Vitaliy Dobriyan")]
        [Test("Rest response deserialization")]
        public void TestRestResponseDeserialization()
        {
            RestDto launchInfo = client.SendRequest(HttpMethod.Get, "/objects/1").As<RestDto>();

            Assert.That(launchInfo.Id, Is.EqualTo("1"));
            Assert.That(launchInfo.Name, Is.EqualTo("Google Pixel 6 Pro"));
        }

        [Author("Evgeniy Voronyuk")]
        [Test("Rest client Get file")]
        public void TestRestClientGetFile()
        {
            string fileName;

            using (Stream stream = new RestClient(Paths.UnicornBaseUrl).GetFileStream(FileEndpoint, out fileName))
            {
                Assert.That(fileName, Is.EqualTo(ExpectedFileName));

                using (StreamReader reader = new StreamReader(stream))
                {
                    Assert.That(reader.ReadToEnd(), Is.EqualTo("This is a test file"));
                }
            }
        }

        [Author("Vitaliy Dobriyan")]
        [Test("Rest client download file")]
        public void TestRestClientDownloadFile()
        {
            string filePath = Path.Combine(Paths.DllFolder, ExpectedFileName);

            new RestClient(Paths.UnicornBaseUrl).DownloadFile(FileEndpoint, Paths.DllFolder);
            Assert.IsTrue(File.Exists(filePath), "File wasn't found");
        }
    }
}
