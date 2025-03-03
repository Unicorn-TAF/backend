using System.Net;
using System.Net.Http.Headers;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Unicorn.Backend.Services.SoapService
{
    /// <summary>
    /// Describes base REST service response object.
    /// </summary>
    public class SoapResponse : ServiceResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SoapResponse"/> class.
        /// </summary>
        public SoapResponse() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoapResponse"/> class with status code, message and headers.
        /// </summary>
        /// <param name="status">response status code</param>
        /// <param name="statusDescription">response status description</param>
        /// <param name="headers">response headers</param>
        public SoapResponse(HttpStatusCode status, string statusDescription, HttpResponseHeaders headers)
            : base(status, statusDescription, headers)
        {
        }

        /// <summary>
        /// Gets response content in form of <see cref="XDocument"/>.
        /// </summary>
        public XDocument AsXDocument
        {
            get
            {
                try
                {
                    XDocument document = XDocument.Parse(Content);

                    if (document.Root.Name.LocalName.Equals("Envelope"))
                    {
                        return new XDocument(document.XPathSelectElement("//*[name()='soap:Body']/*"));
                    }
                        
                    return document;
                }
                catch (XmlException)
                {
                    LogTruncatedContent(Content);
                    throw;
                }
            }
        }
    }
}
