﻿using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using Unicorn.Taf.Core.Logging;

namespace Unicorn.Backend.Services.RestService
{
    /// <summary>
    /// Describes base rest service client containing basic actions.
    /// </summary>
    public class RestClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestClient"/> class with service base url.
        /// </summary>
        /// <param name="baseUrl">service base url</param>
        public RestClient(string baseUrl) : this(baseUrl, null)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RestClient"/> class with service base url 
        /// based on existing session.
        /// </summary>
        /// <param name="baseUri">service base uri</param>
        /// <param name="session">existing service session</param>
        public RestClient(string baseUri, IServiceSession session)
        {
            BaseUri = new Uri(baseUri);
            Session = session;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestClient"/> class.
        /// </summary>
        public RestClient() : this(null, null)
        {
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
        /// Gets content type for service calls.
        /// </summary>
        protected virtual string ContentType { get; } = "application/json";

        /// <summary>
        /// Gets encoding for service calls.
        /// </summary>
        protected virtual Encoding Encoding { get; } = Encoding.UTF8;

        /// <summary>
        /// Sends specified <see cref="HttpRequestMessage"/> request.
        /// </summary>
        /// <param name="request">request to send</param>
        /// <returns>service response</returns>
        public RestResponse SendRequest(HttpRequestMessage request)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Sending {request.Method} request to {request.RequestUri}.");

            using (var handler = new HttpClientHandler())
            {
                handler.AllowAutoRedirect = true;

                // By default HttpClient uses CookieContainer.
                // If we want to set Cookie via Headers we need to disable cookies.
                if (request.Headers.Contains("Cookie"))
                {
                    handler.UseCookies = false;
                }

                using (var client = new HttpClient(handler))
                {
                    var timer = Stopwatch.StartNew();

                    var response = client.SendAsync(request).Result;
                    var responseContent = response.Content.ReadAsStringAsync().Result;

                    var elapsed = timer.Elapsed;

                    var restResponse = new RestResponse(response.StatusCode, response.Headers, response.ReasonPhrase)
                    {
                        Content = responseContent,
                        ExecutionTime = elapsed,
                    };

                    Logger.Instance.Log(LogLevel.Debug, $"Getting {restResponse.Status} response.");

                    if (Logger.Level.Equals(LogLevel.Trace) && restResponse.Content != null)
                    {
                        Logger.Instance.Log(LogLevel.Trace, "Response body: " + restResponse.Content);
                    }

                    request.Dispose();

                    return restResponse;
                }
            }
        }

        /// <summary>
        /// Sends service request type to endpoint with content.
        /// </summary>
        /// <param name="method">Http method</param>
        /// <param name="endpoint">service endpoint relative url</param>
        /// <param name="content">request content</param>
        /// <returns>service response</returns>
        public RestResponse SendRequest(HttpMethod method, string endpoint, string content)
        {
            var request = CreateRequestWithHeaders(method, endpoint, content);

            return SendRequest(request);
        }

        /// <summary>
        /// Sends service request type to endpoint without content.
        /// </summary>
        /// <param name="method">Http method</param>
        /// <param name="endpoint">service endpoint relative url</param>
        /// <returns></returns>
        public RestResponse SendRequest(HttpMethod method, string endpoint) =>
            SendRequest(method, endpoint, string.Empty);

        /// <summary>
        /// Creates <see cref="HttpRequestMessage"/> and fills it's headers from session.
        /// </summary>
        /// <param name="method">Http method</param>
        /// <param name="endpoint">service endpoint relative url</param>
        /// <param name="content">request content</param>
        /// <returns><see cref="HttpRequestMessage"/> instance</returns>
        protected HttpRequestMessage CreateRequestWithHeaders(HttpMethod method, string endpoint, string content)
        {
            var uri = new Uri(BaseUri, endpoint);
            var request = new HttpRequestMessage(method, uri);

            if (!method.Equals(HttpMethod.Get))
            {
                request.Content = new StringContent(content, Encoding, ContentType);
            }

            Session?.UpdateRequestWithSessionData(request);

            return request;
        }
    }
}
