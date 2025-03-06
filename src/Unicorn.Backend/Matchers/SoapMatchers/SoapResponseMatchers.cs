namespace Unicorn.Backend.Matchers.SoapMatchers
{
    /// <summary>
    /// Entry point for SOAP service matchers.
    /// </summary>
    public class SoapResponseMatchers
    {
        /// <summary>
        /// Matcher to check if SOAP service response has a node matching specified XPath.
        /// </summary>
        /// <param name="xPath">XPath to search for node</param>
        /// <returns><see cref="HasNodeMatchingXPathMatcher"/> instance</returns>
        public HasNodeMatchingXPathMatcher HasNodeMatchingXPath(string xPath) =>
            new HasNodeMatchingXPathMatcher(xPath);

        /// <summary>
        /// Matcher to check if SOAP service response has nodes matching specified XPath 
        /// and having specified child nodes.
        /// </summary>
        /// <param name="nodesXpath">XPath to search for parent nodes</param>
        /// <param name="childNodeName">expected child node name</param>
        /// <returns><see cref="EachNodeHasChildMatcher"/> instance</returns>
        public EachNodeHasChildMatcher EachNodeHasChild(string nodesXpath, string childNodeName) =>
            new EachNodeHasChildMatcher(nodesXpath, childNodeName);

        /// <summary>
        /// Matcher to check if SOAP service response has node matching specified XPath 
        /// and having specified value.
        /// </summary>
        /// <param name="xPath">XPath to search for node</param>
        /// <param name="expectedValue">expected node value</param>
        /// <returns><see cref="HasNodeWithValueMatcher"/> instance</returns>
        public HasNodeWithValueMatcher HasNodeWithValue(string xPath, string expectedValue) =>
            new HasNodeWithValueMatcher(xPath, expectedValue);

        /// <summary>
        /// Matcher to check if SOAP service response has specified count of nodes matching XPath .
        /// </summary>
        /// <param name="xPath">XPath to search for nodes</param>
        /// <param name="expectedCount">expected node value</param>
        /// <returns><see cref="HasNodesCountMatcher"/> instance</returns>
        public HasNodesCountMatcher HasNodesCount(string xPath, int expectedCount) =>
            new HasNodesCountMatcher(xPath, expectedCount);
    }
}
