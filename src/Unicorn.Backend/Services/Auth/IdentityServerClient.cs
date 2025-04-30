namespace Unicorn.Backend.Services.Auth
{
    /// <summary>
    /// Represents authorization client for IdentityServer.
    /// The token is retrieved by calling BASE_URL/connect/token
    /// </summary>
    public sealed class IdentityServerClient : OAuth2ClientBase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="IdentityServerClient"/> with base URL to IdentityServer.
        /// </summary>
        /// <param name="baseUrl">base URL to IdentityServer</param>
        public IdentityServerClient(string baseUrl) : base(baseUrl)
        {

        }

        /// <summary>
        /// Gets relative endpoint URL for token retrieval
        /// </summary>
        protected override string Endpoint { get; } = "connect/token";
    }
}
