using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Unicorn.Taf.Core.Logging;

namespace Unicorn.Backend.Services.RestService
{
    /// <summary>
    /// Describes base REST service client containing basic actions.
    /// </summary>
    public class RestClient : ClientBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestClient"/> class with service base url.<br/>
        /// <see cref="SecurityProtocolType.Tls12"/> protocol is used by default.
        /// </summary>
        /// <param name="baseUri">service base url</param>
        public RestClient(string baseUri) : base(baseUri)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestClient"/> class with service base url.<br/>
        /// <see cref="SecurityProtocolType.Tls12"/> protocol is used by default.
        /// </summary>
        /// <param name="baseUri">service base url</param>
        public RestClient(Uri baseUri) : base(baseUri)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestClient"/> class with service base url 
        /// based on existing session.<br/>
        /// <see cref="SecurityProtocolType.Tls12"/> protocol is used by default.
        /// </summary>
        /// <param name="baseUri">service base uri</param>
        /// <param name="session">existing service session</param>
        public RestClient(Uri baseUri, IServiceSession session) : base(baseUri, session)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestClient"/> class.
        /// </summary>
        public RestClient() : base()
        {
        }

        /// <summary>
        /// Gets content type for service calls.
        /// </summary>
        protected override string ContentType { get; } = "application/json";

        /// <summary>
        /// Sends specified <see cref="HttpRequestMessage"/> request.
        /// </summary>
        /// <param name="request">request to send</param>
        /// <returns>service response</returns>
        public virtual RestResponse SendRequest(HttpRequestMessage request)
        {
            ULog.Debug("Sending {0} request to {1}.", request.Method, request.RequestUri);

            Stopwatch timer = Stopwatch.StartNew();
            HttpResponseMessage response = GetClient(request).SendAsync(request).Result;
            string responseContent = response.Content.ReadAsStringAsync().Result;
            TimeSpan elapsed = timer.Elapsed;

            ULog.Debug("Getting {0} response.", response.StatusCode);
            ULog.Trace("Response body: {0}", responseContent);

            RestResponse restResponse = new RestResponse(response.StatusCode, response.ReasonPhrase, response.Headers)
            {
                Content = responseContent,
                ExecutionTime = elapsed,
            };

            request.Dispose();

            return restResponse;
        }

        /// <summary>
        /// Sends service request type to endpoint with content.
        /// </summary>
        /// <param name="method">Http method</param>
        /// <param name="endpoint">service endpoint relative url</param>
        /// <param name="content">request content</param>
        /// <returns>service response</returns>
        public virtual RestResponse SendRequest(HttpMethod method, string endpoint, string content)
        {
            HttpRequestMessage request = CreateRequest(method, endpoint, content);

            return SendRequest(request);
        }

        /// <summary>
        /// Sends service request type to endpoint without content.
        /// </summary>
        /// <param name="method">Http method</param>
        /// <param name="endpoint">service endpoint relative url</param>
        /// <returns></returns>
        public virtual RestResponse SendRequest(HttpMethod method, string endpoint) =>
            SendRequest(method, endpoint, string.Empty);

        /// <summary>
        /// Downloads file from specified endpoint to specified directory with original file name.
        /// </summary>
        /// <param name="endpoint">service endpoint relative url</param>
        /// <param name="destinationFolder">folder to save file to</param>
        /// <returns>downloaded file name</returns>
        public string DownloadFile(string endpoint, string destinationFolder)
        {
            string destinationFileName;

            using (Stream stream = GetFileStream(endpoint, out destinationFileName))
            using (FileStream fileStream = File.Create(Path.Combine(destinationFolder, destinationFileName)))
            {
                stream.CopyTo(fileStream);
            }

            return destinationFileName;
        }

        /// <summary>
        /// Returns <see cref="Stream"/> with file content<br/>
        /// </summary>
        /// <param name="endpoint">service endpoint relative url</param>
        /// <param name="fileName">original remote file name</param>
        /// <returns><see cref="Stream"/> with file content</returns>
        public Stream GetFileStream(string endpoint, out string fileName)
        {
            HttpRequestMessage request = CreateRequest(HttpMethod.Get, endpoint, string.Empty);

            ULog.Debug("Sending {0} request to {1}.", request.Method, request.RequestUri);
            HttpResponseMessage response = GetClient(request).SendAsync(request).Result;
            HttpContent content = response.Content;
            ULog.Debug("Getting {0} response.", response.StatusCode);

            fileName = content.Headers.ContentDisposition?.FileNameStar;

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = endpoint.Split('/').Last();
            }

            fileName = Uri.UnescapeDataString(fileName.Replace("\"", string.Empty));

            return content.ReadAsStreamAsync().Result;
        }
    }
}