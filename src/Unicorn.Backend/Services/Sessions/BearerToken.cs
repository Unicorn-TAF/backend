using System;
using System.Net.Http;

namespace Unicorn.Backend.Services.Sessions
{
    /// <summary>
    /// Represents Bearer token with expiration time. Before a request is sent the token is added to the request
    /// in Authorization header with value "Bearer {token}"
    /// </summary>
    public class BearerToken : IServiceSession
    {
        private readonly string _token;
        private readonly DateTime _expirationDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="BearerToken"/> class with token value and expiration time.<br/>
        /// </summary>
        /// <param name="token">token value</param>
        /// <param name="expiresIn">expiration time as <see cref="TimeSpan"/></param>
        public BearerToken(string token, TimeSpan expiresIn)
        {
            _token = token;
            _expirationDate = DateTime.Now.Add(expiresIn);
        }

        /// <summary>
        /// Indicates whether the token is expired or not
        /// </summary>
        public bool Expired => DateTime.Now > _expirationDate;

        /// <summary>
        /// Updates instance of <see cref="HttpRequestMessage"/> with Authorization header with value "Bearer {token}"
        /// </summary>
        /// <param name="request">service request instance</param>
        public void UpdateRequestWithSessionData(HttpRequestMessage request) =>
            request.Headers.Add("Authorization", "Bearer " + _token);
    }
}
