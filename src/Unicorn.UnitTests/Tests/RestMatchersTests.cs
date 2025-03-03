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
                Response.Rest.ContentContains("expectedBody"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestBodyContainsValueMatcherWithNull() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(null,
                Response.Rest.ContentContains("expectedBody")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestBodyContainsValueMatcherNegative() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(
                GetResponse("{expectedBodyPart}"),
                Response.Rest.ContentContains("NotExpectedBody")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherNegative() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(
                client,
                Service.Rest.HasEndpoint("NotExistingEndpoint")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherNegativeNull() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(
                null,
                Service.Rest.HasEndpoint("NotExistingEndpoint")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherNegativeNoBaseUrl() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(
                new RestClient(),
                Service.Rest.HasEndpoint("weee")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherNegativeNoBaseUrlWithNot() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(
                new RestClient(),
                Is.Not(Service.Rest.HasEndpoint("weee"))));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherNegativeNullWithNot() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(
                null,
                Is.Not(Service.Rest.HasEndpoint("NotExistingEndpoint"))));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherPositive() =>
            Assert.That(
                client,
                Service.Rest.HasEndpoint("/objects/1"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherNegativeNotExistingBaseUrl() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(
                new RestClient("http://bla-bla/"),
                Service.Rest.HasEndpoint("NotExistingEndpoint")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasStatusCodeMatcherPositive() =>
            Assert.That(GetResponse(""), 
                Response.HasStatusCode(HttpStatusCode.OK));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasStatusCodeMatcherWithNull() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(null, 
                Response.HasStatusCode(HttpStatusCode.OK)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasStatusCodeMatcherNegative() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(
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
            TestHelpers.CheckNegativeScenario(() => Assert.That(null,
                Response.Rest.HasTokensCount("$.phoneNumbers", 3)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensCountMatcherNegative() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(
                GetResponse(testJson),
                Response.Rest.HasTokensCount("$.phoneNumbers", 3)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensCountMatcherPositiveNullAssString() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(
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
            TestHelpers.CheckNegativeScenario(() => Assert.That(null,
                Response.Rest.HasTokenWithValue("$.phoneNumbers", 3)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenWithValueMatcherNegativeInt() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(
                GetResponse(testJson), Response.Rest.HasTokenWithValue("$.ageNoToken", "26")));

        ////[Author(Author.VDobriyan)]
        ////[Test("HasTokenWithValueMatcher negative scenario (int)")]
        ////public void TestHasTokenWithValueMatcherNegativeInt() =>
        ////    CheckNegativeScenario(() => Assert.That(
        ////        GetResponse(TestJson), Response.HasTokenWithValue("$.age", "26")));

        ////[Author(Author.VDobriyan)]
        ////[Test("HasTokenWithValueMatcher negative scenario (string)")]
        ////public void TestHasTokenWithValueMatcherNegativeString() =>
        ////    CheckNegativeScenario(() => Assert.That(
        ////        GetResponse(TestJson), Response.HasTokenWithValue("$.ageString", 26)));


        [Test, Author("Vitaliy Dobriyan")]
        public void TestEachTokenHasChildMatcherPositive() =>
            Assert.That(GetResponse(testJson),
                Response.Rest.EachTokenHasChild("$.phoneNumbers[*]", "type"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestEachTokenHasChildMatcherWithNull() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(null,
                Response.Rest.EachTokenHasChild("$.phoneNumbers[*]", "type")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestEachTokenHasChildMatcherNegative() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(
                GetResponse(testJson),
                Response.Rest.EachTokenHasChild("$.phoneNumbers[*]", "subType")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestEachTokenHasChildMatcherNegativeNoToken() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(
                GetResponse(testJson), Service.Rest.Response.EachTokenHasChild(
                    "$.ageNoToken", "26")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenMatchingJsonPathMatcherPositive() =>
            Assert.That(GetResponse(testJson),
                Response.Rest.HasTokenMatchingJsonPath(
                    "$.phoneNumbers[?(@.number == '0123-4567-8910')]"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenMatchingJsonPathMatcherWithNull() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(null,
                Response.Rest.HasTokenMatchingJsonPath(
                    "$.phoneNumbers[?(@.number == '0123-4567-8910')]")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenMatchingJsonPathMatcherNegative() =>
            TestHelpers.CheckNegativeScenario(() => Assert.That(
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
