using System.Net;
using Unicorn.Backend.Matchers;
using Unicorn.Backend.Services.RestService;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.UnitTests.Tests
{
    [Suite]
    public class RestMatchersTests
    {
        private string testJson = DataUtils.GetDataFrom("TestJson.json");
        private string testJArray = DataUtils.GetDataFrom("TestJArray.json");

        private static RestClient client;

        [BeforeSuite]
        public static void SetUp() =>
            client = new RestClient(Paths.ReastApiBaseUrl);

        [AfterSuite]
        public static void TearDown() =>
            client = null;

        [Test, Author("Vitaliy Dobriyan")]
        public void TestBodyContainsValueMatcherPositive() =>
            Assert.That(GetResponse("{expectedBodyPart}"), 
                Response.ContentContains("expectedBody"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestBodyContainsValueMatcherWithNull() =>
            Assert.Throws<AssertionException>(() => Assert.That(null,
                Response.ContentContains("expectedBody")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestBodyContainsValueMatcherNegative() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                GetResponse("{expectedBodyPart}"),
                Response.ContentContains("NotExpectedBody")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherNegative() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                client,
                Service.Rest.HasEndpoint("NotExistingEndpoint")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherNegativeNull() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                null,
                Service.Rest.HasEndpoint("NotExistingEndpoint")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherNegativeNoBaseUrl() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                new RestClient(),
                Service.Rest.HasEndpoint("weee")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherNegativeNoBaseUrlWithNot() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                new RestClient(),
                Is.Not(Service.Rest.HasEndpoint("weee"))));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherNegativeNullWithNot() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                null,
                Is.Not(Service.Rest.HasEndpoint("NotExistingEndpoint"))));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherPositive() =>
            Assert.That(
                client,
                Service.Rest.HasEndpoint("/objects/1"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherNegativeNotExistingBaseUrl() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                new RestClient("http://bla-bla/"),
                Service.Rest.HasEndpoint("NotExistingEndpoint")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasStatusCodeMatcherPositive() =>
            Assert.That(GetResponse(""), 
                Response.HasStatusCode(HttpStatusCode.OK));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasStatusCodeMatcherWithNull() =>
            Assert.Throws<AssertionException>(() => Assert.That(null, 
                Response.HasStatusCode(HttpStatusCode.OK)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasStatusCodeMatcherNegative() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                GetResponse(""), 
                Response.HasStatusCode(HttpStatusCode.Accepted)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensCountMatcherPositive() =>
            Assert.That(GetResponse(testJson), 
                Response.Rest.HasTokensCount("$.phoneNumbers[*]", 2));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensCountMatcherPositiveNoToken() =>
            Assert.That(GetResponse(testJson),
                Response.Rest.HasTokensCount("$.phoneNumbersNotExisting[*]", 0));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensCountMatcherWithNull() =>
            Assert.Throws<AssertionException>(() => Assert.That(null,
                Response.Rest.HasTokensCount("$.phoneNumbers", 3)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensCountMatcherNegative() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                GetResponse(testJson),
                Response.Rest.HasTokensCount("$.phoneNumbers", 3)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensCountMatcherPositiveNullAssString() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                GetResponse(testJson),
                Response.Rest.HasTokenWithValue("$..subType", "null")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenWithValueMatcherPositiveInt() =>
            Assert.That(GetResponse(testJson),
                Response.Rest.HasTokenWithValue("$.age", 26));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenWithValueMatcherPositiveJArray() =>
            Assert.That(GetResponse(testJArray),
                Response.Rest.HasTokenWithValue("$..type", "iPhone"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenWithValueMatcherPositiveString() =>
            Assert.That(GetResponse(testJson),
                Response.Rest.HasTokenWithValue("$.firstName", "John"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenWithValueMatcherWithNull() =>
            Assert.Throws<AssertionException>(() => Assert.That(null,
                Response.Rest.HasTokenWithValue("$.phoneNumbers", 3)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenWithValueMatcherNegativeInt() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                GetResponse(testJson), Response.Rest.HasTokenWithValue("$.ageNoToken", "26")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestEachTokenHasChildMatcherPositive() =>
            Assert.That(GetResponse(testJson),
                Response.Rest.EachTokenHasChild("$.phoneNumbers[*]", "type"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestEachTokenHasChildMatcherWithNull() =>
            Assert.Throws<AssertionException>(() => Assert.That(null,
                Response.Rest.EachTokenHasChild("$.phoneNumbers[*]", "type")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestEachTokenHasChildMatcherNegative() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                GetResponse(testJson),
                Response.Rest.EachTokenHasChild("$.phoneNumbers[*]", "subType")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestEachTokenHasChildMatcherNegativeNoToken() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                GetResponse(testJson), Response.Rest.EachTokenHasChild(
                    "$.ageNoToken", "26")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenMatchingJsonPathMatcherPositive() =>
            Assert.That(GetResponse(testJson),
                Response.Rest.HasTokenMatchingJsonPath(
                    "$.phoneNumbers[?(@.number == '0123-4567-8910')]"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenMatchingJsonPathMatcherWithNull() =>
            Assert.Throws<AssertionException>(() => Assert.That(null,
                Response.Rest.HasTokenMatchingJsonPath(
                    "$.phoneNumbers[?(@.number == '0123-4567-8910')]")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenMatchingJsonPathMatcherNegative() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                GetResponse(testJson),
                Response.Rest.HasTokenMatchingJsonPath(
                    "$.phoneNumbers[?(@.number == '0sdfsdfsd123-4567-8910')]")));


        private static RestResponse GetResponse(string json) =>
            new RestResponse(HttpStatusCode.OK, null, null)
            {
                Content = json
            };
    }
}
