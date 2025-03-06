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

        #region HasNodeMatchingXPath

        [Test, Author("Vitaliy Dobriyan")]
        public void TestEachNodeHasChildMatcherPositive() =>
            Assert.That(response, Response.Soap.EachNodeHasChild("//*[name() = 'NumberToWords']", "ubiNum"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenMatchingJsonPathMatcherNegative() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                response,
                Response.Soap.EachNodeHasChild("//NumberToWords", "ubiNum1")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenMatchingJsonPathMatcherNegativeWithNull() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                null,
                Response.Soap.EachNodeHasChild("//NumberToWords", "ubiNum1")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenMatchingJsonPathMatcherNegativeWithNotWithNull() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                null,
                Is.Not(Response.Soap.EachNodeHasChild("//NumberToWords", "ubiNum1"))));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestEachNodeHasChildMatcherNagativeWhenParentsNotFound() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                response,
                Response.Soap.EachNodeHasChild("//NumberToWordsws", "ubiNum")));

        #endregion

        #region HasNodeMatchingXPath

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasNodeMatchingXPathPositive() =>
            Assert.That(response, Response.Soap.HasNodeMatchingXPath("//NumberToNumber"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasNodeMatchingXPathNegative() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                response,
                Response.Soap.HasNodeMatchingXPath("//NumberToWordss")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasNodeMatchingXPathWithNegationPositive() =>
            Assert.That(response, Is.Not(Response.Soap.HasNodeMatchingXPath("//asdfsdf")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasNodeMatchingXPathWithNegationNegative() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                response,
                Is.Not(Response.Soap.HasNodeMatchingXPath("//NumberToNumber"))));

        #endregion

        #region HasNodesCount

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasNodesCountPositive() =>
            Assert.That(response, Response.Soap.HasNodesCount("//*[name() = 'NumberToWords']", 2));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasNodesCountNegative() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                response,
                Response.Soap.HasNodesCount("//*[name() = 'NumberToWords']", 3)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasNodesCountWithNegationPositive() =>
            Assert.That(response, Is.Not(Response.Soap.HasNodesCount("//asdfsdf", 1)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasNodesCountWithNegationNegative() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                response,
                Is.Not(Response.Soap.HasNodesCount("//*[name() = 'NumberToWords']", 2))));

        #endregion

        #region HasNodeWithValue

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasNodeWithValuePositive() =>
            Assert.That(response, Response.Soap.HasNodeWithValue("//NumberToWords/ubiNum", "500"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasNodeWithValuePositive1() =>
            Assert.That(response, Response.Soap.HasNodeWithValue("//NumberToWords/ubiNum", "400"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasNodeWithValueNegative() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                response,
                Response.Soap.HasNodeWithValue("//NumberToWords/ubiNum", "1500")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasNodeWithValueWithNegationPositive() =>
            Assert.That(response, Is.Not(Response.Soap.HasNodeWithValue("//NumberToWords/ubiNum", "1500")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasNodeWithValueWithNegationNegative() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                response,
                Is.Not(Response.Soap.HasNodeWithValue("//NumberToWords/ubiNum", "500"))));

        #endregion
    }
}
