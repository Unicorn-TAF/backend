namespace Unicorn.Backend.Matchers.SoapMatchers
{
    /// <summary>
    /// Entry point for SOAP service matchers.
    /// </summary>
    public class SoapResponseMatchers
    {
        /// <summary>
        /// Matcher to check if SOAP service response has nodes matching specified XPath 
        /// and having specified child nodes.
        /// </summary>
        /// <param name="nodesXpath">XPath to search for parent nodes</param>
        /// <param name="childNodeName">expected child node name</param>
        /// <returns><see cref="EachNodeHasChildMatcher"/> instance</returns>
        public EachNodeHasChildMatcher EachNodeHasChild(string nodesXpath, string childNodeName) =>
            new EachNodeHasChildMatcher(nodesXpath, childNodeName);
    }
}
