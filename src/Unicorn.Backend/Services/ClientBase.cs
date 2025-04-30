using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Unicorn.Backend.Services
{
    /// <summary>
    /// Describes base rest service client containing basic actions.
    /// </summary>
    public abstract class ClientBase
    {
        private HttpClientHandler handler;
        private HttpClient client;
        private bool ignoreSslErrors = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientBase"/> class with service base url.<br/>
        /// <see cref="SecurityProtocolType.Tls12"/> protocol is used by default.
        /// </summary>
        /// <param name="baseUri">service base url</param>
        protected ClientBase(string baseUri) : this(new Uri(baseUri), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientBase"/> class with service base url.<br/>
        /// <see cref="SecurityProtocolType.Tls12"/> protocol is used by default.
        /// </summary>
        /// <param name="baseUri">service base url</param>
        protected ClientBase(Uri baseUri) : this(baseUri, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientBase"/> class with service base url 
        /// based on existing session.<br/>
        /// <see cref="SecurityProtocolType.Tls12"/> protocol is used by default.
        /// </summary>
        /// <param name="baseUri">service base URI</param>
        /// <param name="session">existing service session</param>
        protected ClientBase(Uri baseUri, IServiceSession session)
        {
            BaseUri = baseUri;
            Session = session;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientBase"/> class.
        /// </summary>
        protected ClientBase()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// Gets or sets service session.
        /// </summary>
        public IServiceSession Session { get; set; }

        /// <summary>
        /// Gets or sets service base url.
        /// </summary>
        public Uri BaseUri { get; set; }

        /// <summary>
        /// Disables SSL verification for all client requests.
        /// </summary>
        public void DisableSslVerification() =>
            ignoreSslErrors = true;

        /// <summary>
        /// Gets content type for service calls.
        /// </summary>
        protected abstract string ContentType { get; }

        /// <summary>
        /// Gets a value indicating whether allow autoredirects or not.
        /// </summary>
        protected virtual bool AllowAutoRedirect { get; } = true;

        /// <summary>
        /// Gets encoding for service calls.
        /// </summary>
        protected virtual Encoding Encoding { get; } = Encoding.UTF8;

        /// <summary>
        /// Creates <see cref="HttpRequestMessage"/> and fills it's headers from session.
        /// </summary>
        /// <param name="method">Http method</param>
        /// <param name="endpoint">service endpoint relative url</param>
        /// <param name="content">request content</param>
        /// <returns><see cref="HttpRequestMessage"/> instance</returns>
        protected virtual HttpRequestMessage CreateRequest(HttpMethod method, string endpoint, string content)
        {
            var uri = new Uri(BaseUri, endpoint);
            var request = new HttpRequestMessage(method, uri);

            string methodLower = method.ToString().ToLowerInvariant();

            if (!methodLower.Equals("get") && !methodLower.Equals("head"))
            {
                request.Content = new StringContent(content, Encoding, ContentType);
            }

            Session?.UpdateRequestWithSessionData(request);

            return request;
        }

        /// <summary>
        /// Gets <see cref="HttpClient"/> instance allowing auto redirects
        /// </summary>
        /// <param name="request">source request instance (used to determine behavior of cookies handling)</param>
        /// <returns></returns>
        protected HttpClient GetClient(HttpRequestMessage request)
        {
            if (client == null)
            {
                handler = new HttpClientHandler();

                if (ignoreSslErrors)
                {
                    handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                }

                handler.AllowAutoRedirect = AllowAutoRedirect;

                // By default HttpClient uses CookieContainer.
                // If we want to set Cookie via Headers we need to disable cookies otherwise CookieContainer is used.
                handler.UseCookies = !request.Headers.Contains("Cookie");

                client = new HttpClient(handler);
            }

            return client;
        }

        /// <summary>
        /// Frees resources taken by <see cref="HttpClientHandler"/> and <see cref="HttpClient"/>
        /// </summary>
        ~ClientBase()
        {
            client?.Dispose();
            handler?.Dispose();
        }
    }
}
