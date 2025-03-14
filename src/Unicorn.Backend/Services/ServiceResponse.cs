﻿using System;
using System.Net;
using System.Net.Http.Headers;
using Unicorn.Taf.Core.Logging;

namespace Unicorn.Backend.Services
{
    /// <summary>
    /// Describes base service response object.
    /// </summary>
    public class ServiceResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse"/> class.
        /// </summary>
        public ServiceResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse"/> class with status code, message and headers.
        /// </summary>
        /// <param name="status">response status code</param>
        /// <param name="statusDescription">response status description</param>
        /// <param name="headers">response headers</param>
        public ServiceResponse(HttpStatusCode status, string statusDescription, HttpResponseHeaders headers)
        {
            Status = status;
            StatusDescription = statusDescription;
            Headers = headers;
        }

        /// <summary>
        /// Gets or sets request execution time.
        /// </summary>
        public TimeSpan ExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets response status.
        /// </summary>
        public HttpStatusCode Status { get; set; }

        /// <summary>
        /// Gets or sets response status description.
        /// </summary>
        public string StatusDescription { get; set; }

        /// <summary>
        /// Gets or sets response content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets response headers.
        /// </summary>
        public HttpResponseHeaders Headers { get; set; }

        /// <summary>
        /// Logs content parsing error and truncates it till 500 symbols.
        /// </summary>
        /// <param name="content">content to log</param>
        protected static void LogTruncatedContent(string content)
        {
            string truncated = content.Length > 500 ?
                    content.Substring(0, 500) + " (content truncated ...)" :
                    content;

            ULog.Error("Unable to parse response as JSON. Content:\n{0}", truncated);
        }
    }
}
