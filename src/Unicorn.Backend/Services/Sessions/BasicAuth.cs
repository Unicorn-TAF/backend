using System;
using System.Net.Http;
using System.Text;

namespace Unicorn.Backend.Services.Sessions
{
    /// <summary>
    /// Represents Basic Auth mechanism. The token value is generated as Base64 of 'username:password'.
    /// Before a request is sent the value is added to the request in Authorization header with value "Basic {token}"
    /// </summary>
    public class BasicAuth : IServiceSession
    {
        private readonly string _credentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicAuth"/> class with token username and password.<br/>
        /// </summary>
        /// <param name="username">username value</param>
        /// <param name="password">password value</param>
        public BasicAuth(string username, string password)
        {
            _credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
        }

        /// <summary>
        /// Updates instance of <see cref="HttpRequestMessage"/> with Authorization header with value "Basic {token}"
        /// </summary>
        /// <param name="request">service request instance</param>
        public void UpdateRequestWithSessionData(HttpRequestMessage request) =>
            request.Headers.Add("Authorization", "Basic " + _credentials);
    }
}
