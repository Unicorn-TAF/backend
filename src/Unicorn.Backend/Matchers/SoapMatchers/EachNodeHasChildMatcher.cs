using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Unicorn.Backend.Services.SoapService;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.Backend.Matchers.SoapMatchers
{
    /// <summary>
    /// Matcher to check if SOAP service response has nodes matching specified XPath 
    /// and having specified child nodes.
    /// </summary>
    public class EachNodeHasChildMatcher : TypeSafeMatcher<SoapResponse>
    {
        private readonly string _nodesXpath;
        private readonly string _childNodeName;

        /// <summary>
        /// Initializes a new instance of the <see cref="EachNodeHasChildMatcher"/> class with XPath and child node name.
        /// </summary>
        /// <param name="nodesXpath">XPath to search for parent nodes</param>
        /// <param name="childNodeName">expected child node name</param>
        public EachNodeHasChildMatcher(string nodesXpath, string childNodeName)
        {
            _nodesXpath = nodesXpath;
            _childNodeName = childNodeName;
        }

        /// <summary>
        /// Gets verification description.
        /// </summary>
        public override string CheckDescription => 
            $"each node in {_nodesXpath} has child '{_childNodeName}'";

        /// <summary>
        /// Checks if target <see cref="SoapResponse"/> matches condition corresponding to specific matcher implementations.
        /// </summary>
        /// <param name="actual">REST response under assertion</param>
        /// <returns>true - if object matches specific condition; otherwise - false</returns>
        public override bool Matches(SoapResponse actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            IEnumerable<XElement> nodes = actual.AsXDocument.XPathSelectElements(_nodesXpath);

            if (!nodes.Any())
            {
                DescribeMismatch("no nodes found by specified XPath");
                return Reverse;
            }

            var hasChild = nodes.All(t => t.XPathSelectElements($"./*[name()='{_childNodeName}']").Any());

            DescribeMismatch(actual.Content);
            return hasChild;
        }
    }
}
