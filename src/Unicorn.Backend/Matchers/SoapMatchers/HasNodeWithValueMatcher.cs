using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Unicorn.Backend.Services.SoapService;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.Backend.Matchers.SoapMatchers
{
    /// <summary>
    /// Matcher to check if SOAP service response has any child matching specified 
    /// XPath and having specified value.
    /// </summary>
    public class HasNodeWithValueMatcher : TypeSafeMatcher<SoapResponse>
    {
        private readonly string _xPath;
        private readonly string _nodeValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="HasNodeWithValueMatcher"/> class with XPath and node value.
        /// </summary>
        /// <param name="xPath">XPath to search for nodes</param>
        /// <param name="nodeValue">expected node value</param>
        public HasNodeWithValueMatcher(string xPath, string nodeValue)
        {
            _xPath = xPath;
            _nodeValue = nodeValue;
        }

        /// <summary>
        /// Gets verification description.
        /// </summary>
        public override string CheckDescription =>
            $"has node '{_xPath}' with value '{_nodeValue}'";

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

            IEnumerable<XElement> nodes = actual.AsXDocument.XPathSelectElements(_xPath);

            DescribeMismatch(actual.Content);
            return nodes.Any(n => n.Value.Equals(_nodeValue));
        }
    }
}
