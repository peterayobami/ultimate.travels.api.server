namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The model representation of Amadeus configuration
    /// </summary>
    public class AmadeusConfiguration
    {
        /// <summary>
        /// The host URL of Amadeus
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// The Amadeus client id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The Amadeus client secret
        /// </summary>
        public string ClientSecret { get; set; }
    }
}