using System;
using System.Collections.Generic;
using System.Net.Http;
using Unicorn.Backend.Services.RestService;
using Unicorn.Backend.Services.Sessions;
using Unicorn.Taf.Core.Logging;

namespace Unicorn.Backend.Services.Auth
{
    /// <summary>
    /// Base for OAuth2 authorization clients implementations.
    /// </summary>
    public abstract class OAuth2ClientBase : RestClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2ClientBase"/> with service base url.
        /// </summary>
        /// <param name="baseUrl">Base URL to authorization service</param>
        protected OAuth2ClientBase(string baseUrl) : base(baseUrl)
        {

        }

        /// <summary>
        /// Gets auth request content type as "application/x-www-form-urlencoded"
        /// </summary>
        protected override string ContentType { get; } = "application/x-www-form-urlencoded";

        /// <summary>
        /// Gets token retrieval endpoint.
        /// </summary>
        protected abstract string Endpoint { get; }

        /// <summary>
        /// Gets token using standard endpoint based on provided authorization data.
        /// </summary>
        /// <param name="authData">authorization data</param>
        /// <returns>session instance with a bearer token</returns>
        public IServiceSession GetToken(OAuth2Data authData) =>
            GetToken(Endpoint, GetDictionaryFromOAuthData(authData));

        /// <summary>
        /// Gets token using specified token endpoint and based on provided authorization parameters dictionary.
        /// </summary>
        /// <param name="endpoint">token endpoint</param>
        /// <param name="parameters">OAuth2 parameters dictionary</param>
        /// <returns>session instance with a bearer token</returns>
        public IServiceSession GetToken(string endpoint, Dictionary<string, string> parameters)
        {
            ULog.Debug("Getting new API token...");

            var uri = new Uri(BaseUri, endpoint);

            var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new FormUrlEncodedContent(parameters)
            };

            RestResponse response = SendRequest(request);

            string token = response.AsJObject.Value<string>("access_token");
            int expiresIn = response.AsJObject.Value<int>("expires_in");
            BearerToken session = new BearerToken(token, TimeSpan.FromSeconds(expiresIn));

            return session;
        }

        /// <summary>
        /// Gets token using standard endpoint by "password" grant and provided username and password.
        /// </summary>
        /// <param name="userName">"username" value</param>
        /// <param name="password">"password" value</param>
        /// <returns>session instance with a bearer token</returns>
        public IServiceSession GetTokenByPassword(string userName, string password) =>
            GetToken(OAuth2Data.FromPassword(userName, password));

        /// <summary>
        /// Gets token using standard endpoint by "authorization_code" grant and provided code.
        /// </summary>
        /// <param name="code">"code" value</param>
        /// <returns>session instance with a bearer token</returns>
        public IServiceSession GetTokenByAuthorizationCode(string code) =>
            GetToken(OAuth2Data.FromAuthorizationCode(code));

        /// <summary>
        /// Gets token using standard endpoint by "client_credentials" grant and provided client_id and client_secret.
        /// </summary>
        /// <param name="clientId">"client_id" value</param>
        /// <param name="clientSecret">"client_secret" value</param>
        /// <returns>session instance with a bearer token</returns>
        public IServiceSession GetTokenByClientCredentials(string clientId, string clientSecret) =>
            GetToken(OAuth2Data.FromClientCredentials(clientId, clientSecret));

        /// <summary>
        /// Gets token using standard endpoint by "client_credentials" grant and provided client_id, client_secret and scope.
        /// </summary>
        /// <param name="clientId">"client_id" value</param>
        /// <param name="clientSecret">"client_secret" value</param>
        /// <param name="scope">"scope" value</param>
        /// <returns>session instance with a bearer token</returns>
        public IServiceSession GetTokenByClientCredentials(string clientId, string clientSecret, string scope) =>
            GetToken(OAuth2Data.FromClientCredentials(clientId, clientSecret, scope));

        private static Dictionary<string, string> GetDictionaryFromOAuthData(OAuth2Data authData)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            TryAddParameter(parameters, "grant_type", authData.GrantType);
            TryAddParameter(parameters, "client_id", authData.ClientId);
            TryAddParameter(parameters, "username", authData.Username);
            TryAddParameter(parameters, "password", authData.Password);
            TryAddParameter(parameters, "client_secret", authData.ClientSecret);
            TryAddParameter(parameters, "scope", authData.Scope);
            TryAddParameter(parameters, "code", authData.Code);

            return parameters;
        }

        private static void TryAddParameter(Dictionary<string, string> parameters, string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                parameters.Add(key, value);
            }
        }
    }
}
