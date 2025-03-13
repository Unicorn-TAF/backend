using System.Linq;
using System.Xml.XPath;
using Unicorn.Backend.Services.SoapService;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.Backend.Matchers.SoapMatchers
{
    /// <summary>
    /// Matcher to check if SOAP response has expected number of nodes 
    /// matching specified XPath.
    /// </summary>
    public class HasNodesCountMatcher : TypeSafeMatcher<SoapResponse>
    {
        private readonly string _xPath;
        private readonly int _expectedCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="HasNodesCountMatcher"/> class with XPath and nodes count.
        /// </summary>
        /// <param name="xPath">XPath to search for nodes</param>
        /// <param name="expectedCount">expected nodes count</param>
        public HasNodesCountMatcher(string xPath, int expectedCount)
        {
            _xPath = xPath;
            _expectedCount = expectedCount;
        }

        /// <summary>
        /// Gets verification description.
        /// </summary>
        public override string CheckDescription =>
            $"has {_expectedCount} nodes with XPath '{_xPath}'";

        /// <summary>
        /// Checks if target <see cref="SoapResponse"/> matches condition corresponding to specific matcher implementations.
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

            int nodesCount = actual.AsXDocument.XPathSelectElements(_xPath).Count();

            bool hasCount = nodesCount == _expectedCount;

            DescribeMismatch(nodesCount.ToString());
            return hasCount;
        }
    }
}
