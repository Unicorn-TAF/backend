namespace Unicorn.Backend.Services.Auth
{
    /// <summary>
    /// Represents authorization client for Keycloak. 
    /// The token is retrieved by calling BASE_URL/auth/realms/[realmName]/protocol/openid-connect/token
    /// </summary>
    public sealed class KeycloakClient : OAuth2ClientBase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="KeycloakClient"/> with base URL to Keycloak and realm name.
        /// </summary>
        /// <param name="baseUrl">base URL to Keycloak</param>
        /// <param name="realmName">target realm name (used to generate relative path for token retrieval)</param>
        public KeycloakClient(string baseUrl, string realmName) : base(baseUrl)
        {
            Endpoint = $"auth/realms/{realmName}/protocol/openid-connect/token";
        }

        /// <summary>
        /// Gets relative endpoint URL for token retrieval
        /// </summary>
        protected override string Endpoint { get; }
    }
}
