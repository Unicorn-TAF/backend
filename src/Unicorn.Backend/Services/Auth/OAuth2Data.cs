namespace Unicorn.Backend.Services.Auth
{
    /// <summary>
    /// Class representing OAuth2 properties for token retrieval.
    /// </summary>
    public class OAuth2Data
    {
        /// <summary>
        /// Gets or sets "username" property.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets "password" property.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets "grant_type" property.
        /// </summary>
        public string GrantType { get; set; }

        /// <summary>
        /// Gets or sets "scope" property.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Gets or sets "code" property.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets "client_id" property.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets "client_secret" property.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Creates new <see cref="OAuth2Data"/> with "password" grant type and "username" and "password" values.
        /// </summary>
        /// <param name="username">"username" value</param>
        /// <param name="password">"password" value</param>
        /// <returns>instance of <see cref="OAuth2Data"/></returns>
        public static OAuth2Data FromPassword(string username, string password) =>
            new OAuth2Data()
            {
                GrantType = "password",
                Username = username,
                Password = password
            };

        /// <summary>
        /// Creates new <see cref="OAuth2Data"/> with "authorization_code" grant type and "code" value.
        /// </summary>
        /// <param name="code">"code" value</param>
        /// <returns>instance of <see cref="OAuth2Data"/></returns>
        public static OAuth2Data FromAuthorizationCode(string code) =>
            new OAuth2Data()
            {
                GrantType = "authorization_code",
                Code = code
            };

        /// <summary>
        /// Creates new <see cref="OAuth2Data"/> with "client_credentials" grant type 
        /// and "client_id" and "client_secret" values.
        /// </summary>
        /// <param name="clientId">"client_id" value</param>
        /// <param name="clientSecret">"client_secret" value</param>
        /// <returns>instance of <see cref="OAuth2Data"/></returns>
        public static OAuth2Data FromClientCredentials(string clientId, string clientSecret) =>
            new OAuth2Data()
            {
                GrantType = "client_credentials",
                ClientId = clientId,
                ClientSecret = clientSecret
            };

        /// <summary>
        /// Creates new <see cref="OAuth2Data"/> with "client_credentials" grant type 
        /// and "client_id", "client_secret" and "scope" values.
        /// </summary>
        /// <param name="clientId">"client_id" value</param>
        /// <param name="clientSecret">"client_secret" value</param>
        /// <param name="scope">"scope" value</param>
        /// <returns>instance of <see cref="OAuth2Data"/></returns>
        public static OAuth2Data FromClientCredentials(string clientId, string clientSecret, string scope)
        {
            OAuth2Data data = FromClientCredentials(clientId, clientSecret);
            data.Scope = scope;
            return data;
        }
    }
}
