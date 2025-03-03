using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;

namespace Unicorn.Backend.Services.RestService
{
    /// <summary>
    /// Describes base REST service response object.
    /// </summary>
    public class RestResponse : ServiceResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestResponse"/> class.
        /// </summary>
        public RestResponse() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestResponse"/> class with status code, message and headers.
        /// </summary>
        /// <param name="status">response status code</param>
        /// <param name="statusDescription">response status description</param>
        /// <param name="headers">response headers</param>
        public RestResponse(HttpStatusCode status, string statusDescription, HttpResponseHeaders headers)
            : base(status, statusDescription, headers)
        {
        }

        /// <summary>
        /// Gets response content in form of <see cref="JObject"/>.
        /// </summary>
        public JObject AsJObject
        {
            get
            {
                try
                {
                    return JObject.Parse(Content);
                }
                catch (JsonReaderException)
                {
                    LogTruncatedContent(Content);
                    throw;
                }
            }
        }

        /// <summary>
        /// Deserializes response to specified type.
        /// </summary>
        /// <typeparam name="T">type to deserialize to</typeparam>
        /// <returns>deserialized object instance</returns>
        /// <exception cref="JsonReaderException"/>
        public T As<T>()
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(Content);
            }
            catch (JsonReaderException)
            {
                LogTruncatedContent(Content);
                throw;
            }
        }
    }
}
