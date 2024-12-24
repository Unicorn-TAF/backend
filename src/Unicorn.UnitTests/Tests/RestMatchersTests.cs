using System;
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
        private const string TestJson = @"{
  ""firstName"": ""John"",
  ""lastName"" : ""doe"",
  ""age""      : 26,
  ""ageString""      : ""26"",
  ""address""  : {
    ""streetAddress"": ""naist street"",
    ""city""         : ""Nara"",
    ""postalCode""   : ""630-0192""
  },
  ""phoneNumbers"": [
    {
      ""type""  : ""iPhone"",
      ""number"": ""0123-4567-8888""
    },
    {
      ""type""  : ""home"",
      ""subType"" : ""null"",
      ""number"": ""0123-4567-8910""
    }
  ]
}";

        private const string TestJArray = @"[
  {
    ""type""  : ""iPhone"",
    ""number"": ""0123-4567-8888""
  },
  {
    ""type""  : ""home"",
    ""subType"" : ""kitchen"",
    ""number"": ""0123-4567-8910""
  }
]";

        private static RestClient client;

        [BeforeSuite]
        public static void SetUp() =>
            client = new RestClient(Paths.ApiBaseUrl);

        [AfterSuite]
        public static void TearDown() =>
            client = null;

        [Test, Author("Vitaliy Dobriyan")]
        public void TestBodyContainsValueMatcherPositive() =>
            Assert.That(GetResponse("{expectedBodyPart}"), 
                Service.Rest.Response.ContentContains("expectedBody"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestBodyContainsValueMatcherWithNull() =>
            CheckNegativeScenario(() => Assert.That(null, 
                Service.Rest.Response.ContentContains("expectedBody")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestBodyContainsValueMatcherNegative() =>
            CheckNegativeScenario(() => Assert.That(
                GetResponse("{expectedBodyPart}"), 
                Service.Rest.Response.ContentContains("NotExpectedBody")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherNegative() =>
            CheckNegativeScenario(() => Assert.That(
                client,
                Service.Rest.HasEndpoint("NotExistingEndpoint")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherNegativeNull() =>
            CheckNegativeScenario(() => Assert.That(
                null,
                Service.Rest.HasEndpoint("NotExistingEndpoint")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherNegativeNoBaseUrl() =>
            CheckNegativeScenario(() => Assert.That(
                new RestClient(),
                Service.Rest.HasEndpoint("weee")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherNegativeNoBaseUrlWithNot() =>
            CheckNegativeScenario(() => Assert.That(
                new RestClient(),
                Is.Not(Service.Rest.HasEndpoint("weee"))));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherNegativeNullWithNot() =>
            CheckNegativeScenario(() => Assert.That(
                null,
                Is.Not(Service.Rest.HasEndpoint("NotExistingEndpoint"))));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherPositive() =>
            Assert.That(
                client,
                Service.Rest.HasEndpoint("/!api/internal/repositories/dobriyanchik/unicorntaf/metadata"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasEndpointMatcherNegativeNotExistingBaseUrl() =>
            CheckNegativeScenario(() => Assert.That(
                new RestClient("http://bla-bla/"),
                Service.Rest.HasEndpoint("NotExistingEndpoint")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasStatusCodeMatcherPositive() =>
            Assert.That(GetResponse(""), 
                Service.Rest.Response.HasStatusCode(HttpStatusCode.OK));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasStatusCodeMatcherWithNull() =>
            CheckNegativeScenario(() => Assert.That(null, 
                Service.Rest.Response.HasStatusCode(HttpStatusCode.OK)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasStatusCodeMatcherNegative() =>
            CheckNegativeScenario(() => Assert.That(
                GetResponse(""), 
                Service.Rest.Response.HasStatusCode(HttpStatusCode.Accepted)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensCountMatcherPositive() =>
            Assert.That(GetResponse(TestJson), 
                Service.Rest.Response.HasTokensCount("$.phoneNumbers[*]", 2));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensCountMatcherPositiveNoToken() =>
            Assert.That(GetResponse(TestJson), 
                Service.Rest.Response.HasTokensCount("$.phoneNumbersNotExisting[*]", 0));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensCountMatcherWithNull() =>
            CheckNegativeScenario(() => Assert.That(null, 
                Service.Rest.Response.HasTokensCount("$.phoneNumbers", 3)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensCountMatcherNegative() =>
            CheckNegativeScenario(() => Assert.That(
                GetResponse(TestJson), 
                Service.Rest.Response.HasTokensCount("$.phoneNumbers", 3)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokensCountMatcherPositiveNullAssString() =>
            CheckNegativeScenario(() => Assert.That(
                GetResponse(TestJson), 
                Service.Rest.Response.HasTokenWithValue("$..subType", "null")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenWithValueMatcherPositiveInt() =>
            Assert.That(GetResponse(TestJson), 
                Service.Rest.Response.HasTokenWithValue("$.age", 26));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenWithValueMatcherPositiveJArray() =>
            Assert.That(GetResponse(TestJArray), 
                Service.Rest.Response.HasTokenWithValue("$..type", "iPhone"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenWithValueMatcherPositiveString() =>
            Assert.That(GetResponse(TestJson), 
                Service.Rest.Response.HasTokenWithValue("$.firstName", "John"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenWithValueMatcherWithNull() =>
            CheckNegativeScenario(() => Assert.That(null, 
                Service.Rest.Response.HasTokenWithValue("$.phoneNumbers", 3)));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenWithValueMatcherNegativeInt() =>
            CheckNegativeScenario(() => Assert.That(
                GetResponse(TestJson), Service.Rest.Response.HasTokenWithValue("$.ageNoToken", "26")));

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
            Assert.That(GetResponse(TestJson), 
                Service.Rest.Response.EachTokenHasChild("$.phoneNumbers[*]", "type"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestEachTokenHasChildMatcherWithNull() =>
            CheckNegativeScenario(() => Assert.That(null, 
                Service.Rest.Response.EachTokenHasChild("$.phoneNumbers[*]", "type")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestEachTokenHasChildMatcherNegative() =>
            CheckNegativeScenario(() => Assert.That(
                GetResponse(TestJson), 
                Service.Rest.Response.EachTokenHasChild("$.phoneNumbers[*]", "subType")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestEachTokenHasChildMatcherNegativeNoToken() =>
            CheckNegativeScenario(() => Assert.That(
                GetResponse(TestJson), Service.Rest.Response.EachTokenHasChild(
                    "$.ageNoToken", "26")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenMatchingJsonPathMatcherPositive() =>
            Assert.That(GetResponse(TestJson), 
                Service.Rest.Response.HasTokenMatchingJsonPath(
                    "$.phoneNumbers[?(@.number == '0123-4567-8910')]"));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenMatchingJsonPathMatcherWithNull() =>
            CheckNegativeScenario(() => Assert.That(null, 
                Service.Rest.Response.HasTokenMatchingJsonPath(
                    "$.phoneNumbers[?(@.number == '0123-4567-8910')]")));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestHasTokenMatchingJsonPathMatcherNegative() =>
            CheckNegativeScenario(() => Assert.That(
                GetResponse(TestJson), 
                Service.Rest.Response.HasTokenMatchingJsonPath(
                    "$.phoneNumbers[?(@.number == '0sdfsdfsd123-4567-8910')]")));

        private void CheckNegativeScenario(Action action)
        {
            try
            {
                action();
                throw new InvalidOperationException("Assertion was passed but shouldn't.");
            }
            catch (AssertionException)
            {
                // positive scenario.
            }
        }

        private static RestResponse GetResponse(string json) =>
            new RestResponse(HttpStatusCode.OK, null, null)
            {
                Content = json
            };
    }
}
