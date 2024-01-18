namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The amadeus credentials database table representational model
    /// </summary>
    public class AmadeusCredentialsDataModel : BaseDataModel
    {
        /// <summary>
        /// The type of the token
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The username of the client the token was assigned to
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The client id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The token type
        /// </summary>
        public string TokenType { get; set; }

        /// <summary>
        /// The access token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// The expiration duration in seconds
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// The state of the token, approved or expired
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The scope of the token
        /// </summary>
        public string Scope { get; set; }
    }
}