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
                ApiResponse.ContentContains("expectedBody"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestBodyContainsValueMatcherWithNull() =>
            Assert.Throws<AssertionException>(() => Assert.That(null,
                ApiResponse.ContentContains("expectedBody")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestBodyContainsValueMatcherNegative() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                GetResponse("{expectedBodyPart}"),
                ApiResponse.ContentContains("NotExpectedBody")));

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
                ApiResponse.HasStatusCode(HttpStatusCode.OK));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasStatusCodeMatcherWithNull() =>
            Assert.Throws<AssertionException>(() => Assert.That(null, 
                ApiResponse.HasStatusCode(HttpStatusCode.OK)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasStatusCodeMatcherNegative() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                GetResponse(""), 
                ApiResponse.HasStatusCode(HttpStatusCode.Accepted)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensCountMatcherPositive() =>
            Assert.That(GetResponse(testJson), 
                ApiResponse.Rest.HasTokensCount("$.phoneNumbers[*]", 2));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensCountMatcherPositiveNoToken() =>
            Assert.That(GetResponse(testJson),
                ApiResponse.Rest.HasTokensCount("$.phoneNumbersNotExisting[*]", 0));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensCountMatcherWithNull() =>
            Assert.Throws<AssertionException>(() => Assert.That(null,
                ApiResponse.Rest.HasTokensCount("$.phoneNumbers", 3)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensCountMatcherNegative() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                GetResponse(testJson),
                ApiResponse.Rest.HasTokensCount("$.phoneNumbers", 3)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensCountMatcherPositiveNullAssString() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                GetResponse(testJson),
                ApiResponse.Rest.HasTokenWithValue("$..subType", "null")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenWithValueMatcherPositiveInt() =>
            Assert.That(GetResponse(testJson),
                ApiResponse.Rest.HasTokenWithValue("$.age", 26));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenWithValueMatcherPositiveJArray() =>
            Assert.That(GetResponse(testJArray),
                ApiResponse.Rest.HasTokenWithValue("$..type", "iPhone"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenWithValueMatcherPositiveString() =>
            Assert.That(GetResponse(testJson),
                ApiResponse.Rest.HasTokenWithValue("$.firstName", "John"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenWithValueMatcherWithNull() =>
            Assert.Throws<AssertionException>(() => Assert.That(null,
                ApiResponse.Rest.HasTokenWithValue("$.phoneNumbers", 3)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenWithValueMatcherNegativeInt() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                GetResponse(testJson), ApiResponse.Rest.HasTokenWithValue("$.ageNoToken", "26")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestEachTokenHasChildMatcherPositive() =>
            Assert.That(GetResponse(testJson),
                ApiResponse.Rest.EachTokenHasChild("$.phoneNumbers[*]", "type"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestEachTokenHasChildMatcherWithNull() =>
            Assert.Throws<AssertionException>(() => Assert.That(null,
                ApiResponse.Rest.EachTokenHasChild("$.phoneNumbers[*]", "type")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestEachTokenHasChildMatcherNegative() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                GetResponse(testJson),
                ApiResponse.Rest.EachTokenHasChild("$.phoneNumbers[*]", "subType")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestEachTokenHasChildMatcherNegativeNoToken() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                GetResponse(testJson), ApiResponse.Rest.EachTokenHasChild(
                    "$.ageNoToken", "26")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenMatchingJsonPathMatcherPositive() =>
            Assert.That(GetResponse(testJson),
                ApiResponse.Rest.HasTokenMatchingJsonPath(
                    "$.phoneNumbers[?(@.number == '0123-4567-8910')]"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenMatchingJsonPathMatcherWithNull() =>
            Assert.Throws<AssertionException>(() => Assert.That(null,
                ApiResponse.Rest.HasTokenMatchingJsonPath(
                    "$.phoneNumbers[?(@.number == '0123-4567-8910')]")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenMatchingJsonPathMatcherNegative() =>
            Assert.Throws<AssertionException>(() => Assert.That(
                GetResponse(testJson),
                ApiResponse.Rest.HasTokenMatchingJsonPath(
                    "$.phoneNumbers[?(@.number == '0sdfsdfsd123-4567-8910')]")));


        #region HasTokensListWithValuesMatcher

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensListWithValuesMatcherPositiveInt() =>
            Assert.That(GetResponse(testJArray),
                ApiResponse.Rest.HasTokensListWithValues("$..id", 1, 2));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensListWithValuesMatcherPositiveIntWithNegation() =>
            Assert.That(GetResponse(testJArray),
                Is.Not(ApiResponse.Rest.HasTokensListWithValues("$..id", 3, 2)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensListWithValuesMatcherNegativeIntWithDuplication() =>
            Assert.Throws<AssertionException>(() => Assert.That(GetResponse(testJArray),
                ApiResponse.Rest.HasTokensListWithValues("$..id", 2, 2)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensListWithValuesMatcherNegativeIntWithEmptyActuals() =>
            Assert.Throws<AssertionException>(() => Assert.That(GetResponse(testJArray),
                ApiResponse.Rest.HasTokensListWithValues("$..dsafid", 2, 2)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensListWithValuesMatcherNegativeIntWithEmptyExpecteds() =>
            Assert.Throws<AssertionException>(() => Assert.That(GetResponse(testJArray),
                ApiResponse.Rest.HasTokensListWithValues("$..id", new int[0])));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensListWithValuesMatcherWithNull() =>
            Assert.Throws<AssertionException>(() => Assert.That(null,
                ApiResponse.Rest.HasTokensListWithValues("$..type", "home", "iPhone")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensListWithValuesMatcherWithNullWithNegation() =>
            Assert.Throws<AssertionException>(() => Assert.That(null,
                Is.Not(ApiResponse.Rest.HasTokensListWithValues("$..type", "home", "iPhone"))));

        #endregion

        private static RestResponse GetResponse(string json) =>
            new RestResponse(HttpStatusCode.OK, null, null)
            {
                Content = json
            };
    }
}
