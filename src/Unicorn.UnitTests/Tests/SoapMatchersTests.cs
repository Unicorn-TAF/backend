using Unicorn.Backend.Matchers;
using Unicorn.Backend.Services.SoapService;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.UnitTests.Tests
{
    [Suite]
    public class SoapMatchersTests
    {
        private SoapResponse response;

        [BeforeSuite]
        public void SetUp() =>
            response = new SoapResponse
            {
                Content = DataUtils.GetDataFrom("TextXmlBody.xml")
            };

        [AfterSuite]
        public void TearDown() =>
            response = null;

        [Test, Author("Vitaliy Dobriyan")]
        public void TestEachNodeHasChildMatcherPositive() =>
            Assert.That(response, Response.Soap.EachNodeHasChild("//*[name() = 'NumberToWords']", "ubiNum"));


        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenMatchingJsonPathMatcherNegative() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(
                response,
                Response.Soap.EachNodeHasChild("//NumberToWords", "ubiNum1")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenMatchingJsonPathMatcherNegativeWithNull() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(
                null,
                Response.Soap.EachNodeHasChild("//NumberToWords", "ubiNum1")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenMatchingJsonPathMatcherNegativeWithNotWithNull() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(
                null,
                Is.Not(Response.Soap.EachNodeHasChild("//NumberToWords", "ubiNum1"))));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestEachNodeHasChildMatcherNagativeWhenParentsNotFound() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(
                response,
                Response.Soap.EachNodeHasChild("//NumberToWordsws", "ubiNum")));
    }
}
