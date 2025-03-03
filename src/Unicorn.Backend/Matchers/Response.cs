using System.Net;
using Unicorn.Backend.Matchers.RestMatchers;
using Unicorn.Backend.Matchers.SoapMatchers;

namespace Unicorn.Backend.Matchers
{
    /// <summary>
    /// Entry point for Web Service matchers.
    /// </summary>
    public static class Response
    {
        /// <summary>
        /// Gets entry point for REST response matchers.
        /// </summary>
        public static RestResponseMatchers Rest { get; } = new RestResponseMatchers();


        /// <summary>
        /// Gets entry point for SOAP response matchers.
        /// </summary>
        public static SoapResponseMatchers Soap { get; } = new SoapResponseMatchers();

        /// <summary>
        /// Matcher to check if web service response has specified status code.
        /// </summary>
        /// <param name="statusCode">expected status code</param>
        /// <returns><see cref="HasStatusCodeMatcher"/> instance</returns>
        public static HasStatusCodeMatcher HasStatusCode(HttpStatusCode statusCode) =>
            new HasStatusCodeMatcher(statusCode);

        /// <summary>
        /// Matcher to check if web service response content contains substring.
        /// </summary>
        /// <param name="expectedSubstring">substring to search for in response content</param>
        /// <returns><see cref="ContentContainsMatcher"/> instance</returns>
        public static ContentContainsMatcher ContentContains(string expectedSubstring) =>
            new ContentContainsMatcher(expectedSubstring);
    }
}

