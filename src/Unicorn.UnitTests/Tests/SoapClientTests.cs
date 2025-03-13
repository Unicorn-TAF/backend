using System;
using System.Linq;
using System.Net;
using Unicorn.Backend.Services.SoapService;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.UnitTests.Tests
{
    [Suite]
    public class SoapClientTests
    {
        private static SoapClient client;

        [BeforeSuite]
        public static void SetUp() =>
            client = new SoapClient(Paths.SoapBaseUrl);

        [AfterSuite]
        public static void TearDown() =>
                client = null;

        [Author("Vitaliy Dobriyan")]
        [Test("SOAP client sends correct post request with XML body")]
        public void TestSoapClientCorrectPostRequestWithXmlBody()
        {
            SoapResponse response = client.Post(
                "/webservicesserver/NumberConversion.wso",
                DataUtils.GetDataFrom("SoapRequestBody.xml"));

            CheckResponse(response);
            Assert.That(response.Content, Text.Contains("<m:NumberToWordsResult>five hundred </m:NumberToWordsResult>"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test("SOAP client sends correct get request with url params")]
        public void TestSoapClientCorrectGetRequestWithUrlParams()
        {
            SoapResponse response = client.Get("/webservicesserver/NumberConversion.wso/NumberToWords?ubiNum=234");
            CheckResponse(response);
            Assert.That(response.Content, Text.Contains("<string>two hundred and thirty four </string>"));
        }

        private static void CheckResponse(SoapResponse response)
        {
            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.ExecutionTime, Is.IsLessThan(TimeSpan.FromSeconds(5)));

            Assert.IsTrue(response.Headers.GetValues("web-service").Any(h => h.Contains("DataFlex 19.1")),
                "response does not contain expected header");
        }
    }
}
