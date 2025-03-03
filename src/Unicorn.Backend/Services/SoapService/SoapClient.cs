using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using Unicorn.Taf.Core.Logging;

namespace Unicorn.Backend.Services.SoapService
{
    /// <summary>
    /// Describes base SOAP service client containing basic actions.
    /// </summary>
    public class SoapClient : ClientBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SoapClient"/> class with service base url.<br/>
        /// <see cref="SecurityProtocolType.Tls12"/> protocol is used by default.
        /// </summary>
        /// <param name="baseUri">service base url</param>
        public SoapClient(string baseUri) : base(baseUri)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoapClient"/> class with service base url.<br/>
        /// <see cref="SecurityProtocolType.Tls12"/> protocol is used by default.
        /// </summary>
        /// <param name="baseUri">service base url</param>
        public SoapClient(Uri baseUri) : base(baseUri)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoapClient"/> class with service base url 
        /// based on existing session.<br/>
        /// <see cref="SecurityProtocolType.Tls12"/> protocol is used by default.
        /// </summary>
        /// <param name="baseUri">service base uri</param>
        /// <param name="session">existing service session</param>
        public SoapClient(Uri baseUri, IServiceSession session) : base(baseUri, session)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoapClient"/> class.
        /// </summary>
        public SoapClient() : base()
        {
        }

        /// <summary>
        /// Gets content type for service calls.
        /// </summary>
        protected override string ContentType { get; } = "text/xml";

        /// <summary>
        /// Sends specified <see cref="HttpRequestMessage"/> request.
        /// </summary>
        /// <param name="request">request to send</param>
        /// <returns>service response</returns>
        public virtual SoapResponse SendRequest(HttpRequestMessage request)
        {
            ULog.Debug("Sending {0} request to {1}.", request.Method, request.RequestUri);

            Stopwatch timer = Stopwatch.StartNew();
            HttpResponseMessage response = GetClient(request).SendAsync(request).Result;
            
            string responseContent = response.Content.ReadAsStringAsync().Result;
            TimeSpan elapsed = timer.Elapsed;

            ULog.Debug("Getting {0} response.", response.StatusCode);
            ULog.Trace("Response body: {0}", responseContent);

            SoapResponse soapResponse = new SoapResponse(response.StatusCode, response.ReasonPhrase, response.Headers)
            {
                Content = responseContent,
                ExecutionTime = elapsed,
            };

            request.Dispose();

            return soapResponse;
        }

        /// <summary>
        /// Sends POST request with content to specified endpoint.
        /// </summary>
        /// <param name="endpoint">service endpoint relative url</param>
        /// <param name="content">request content</param>
        /// <returns>service response</returns>
        public virtual SoapResponse Post(string endpoint, string content)
        {
            HttpRequestMessage request = CreateRequest(HttpMethod.Post, endpoint, content);
            return SendRequest(request);
        }

        /// <summary>
        /// Sends GET request to endpoint without content.
        /// </summary>
        /// <param name="endpoint">service endpoint relative url</param>
        /// <returns>service response</returns>
        public virtual SoapResponse Get(string endpoint)
        {
            HttpRequestMessage request = CreateRequest(HttpMethod.Get, endpoint, string.Empty);
            return SendRequest(request);
        }
    }
}