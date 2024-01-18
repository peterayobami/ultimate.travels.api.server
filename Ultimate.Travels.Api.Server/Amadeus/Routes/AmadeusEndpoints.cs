namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The Amadeus API endpoints constants
    /// </summary>
    public class AmadeusEndpoints
    {
        /// <summary>
        /// The endpoint to get access token
        /// </summary>
        public const string GetAccessToken = "/v1/security/oauth2/token";

        /// <summary>
        /// The endpoint to get flight offers
        /// </summary>
        public const string GetFlightOffers = "/v2/shopping/flight-offers";

        /// <summary>
        /// The endpoint to verify offers price
        /// </summary>
        public const string VerifyOffersPrice = "/v1/shopping/flight-offers/pricing";

        /// <summary>
        /// The endpoint to book flight
        /// </summary>
        public const string BookFlightOffer = "/v1/booking/flight-orders";
    }
}