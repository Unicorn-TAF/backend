using System.Linq;
using System.Xml.XPath;
using Unicorn.Backend.Services.SoapService;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.Backend.Matchers.SoapMatchers
{
    /// <summary>
    /// Matcher to check if SOAP service response has any child matching specified XPath.
    /// </summary>
    public class HasNodeMatchingXPathMatcher : TypeSafeMatcher<SoapResponse>
    {
        private readonly string _xPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="HasNodeMatchingXPathMatcher"/> class with XPath.
        /// </summary>
        /// <param name="jsonPath">XPath to search for nodes</param>
        public HasNodeMatchingXPathMatcher(string xPath)
        {
            _xPath = xPath;
        }

        /// <summary>
        /// Gets verification description.
        /// </summary>
        public override string CheckDescription => $"has node matching XPath '{_xPath}'";

        /// <summary>
        /// Checks if target <see cref="RestResponse"/> matches condition corresponding to specific matcher implementations.
        /// </summary>
        /// <param name="actual">SOAP response under assertion</param>
        /// <returns>true - if object matches specific condition; otherwise - false</returns>
        public override bool Matches(SoapResponse actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            bool hasMatch = actual.AsXDocument.XPathSelectElements(_xPath).Any();

            DescribeMismatch(actual.Content);
            return hasMatch;
        }
    }
}
