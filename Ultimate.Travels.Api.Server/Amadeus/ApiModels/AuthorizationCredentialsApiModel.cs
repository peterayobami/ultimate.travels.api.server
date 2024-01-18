using System.Text.Json.Serialization;

namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The API model for Amadeus authorization credentials
    /// </summary>
    public class AuthorizationCredentialsApiModel
    {
        /// <summary>
        /// The type of the token
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// The username of the client the token was assigned to
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; }

        /// <summary>
        /// The client id
        /// </summary>
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        /// <summary>
        /// The token type
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// The access token
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// The expiration duration in seconds
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// The state of the token, approved or expired
        /// </summary>
        [JsonPropertyName("state")]
        public string State { get; set; }

        /// <summary>
        /// The scope of the token
        /// </summary>
        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}