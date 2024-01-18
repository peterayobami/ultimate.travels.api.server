namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The service for all Amadeus transactions
    /// </summary>
    public class AmadeusService
    {
        /// <summary>
        /// The scoped instance of the <see cref="AmadeusClient"/>
        /// </summary>
        private readonly AmadeusClient amadeusClient;

        #region Constructor

        public AmadeusService(AmadeusClient amadeusClient)
        {
            this.amadeusClient = amadeusClient;
        }

        #endregion

        /// <summary>
        /// Configures and sends API request to fetch flight offers
        /// </summary>
        /// <param name="model">The flight search credentials</param>
        /// <returns></returns>
        public async Task<AmadeusApiResult<FlightOfferApiModel, WarningResultApiModel, ErrorResultApiModel>> GetOffersAsync(FlightSearchApiModel model, string amaClientRef)
        {
            // Set the request properties
            var request = new AmadeusRequestApiModel<FlightSearchApiModel>
            {
                Endpoint = AmadeusEndpoints.GetFlightOffers,
                Headers = new Dictionary<string, string>
                {
                    { "ama-client-ref", amaClientRef }
                },
                Data = model,
                Method = HttpMethod.Post
            };
            
            // Trigger the request
            var response = await amadeusClient
                .SendAsync<FlightSearchApiModel, FlightOfferApiModel, WarningResultApiModel, ErrorResultApiModel>(request);

            // Return the response
            return response;
        }

        /// <summary>
        /// Configures and sends API request to verify flight offer price
        /// </summary>
        /// <param name="model">The flight search credentials</param>
        /// <returns></returns>
        public async Task<AmadeusApiResult<OffersPriceApiModel, WarningResultApiModel, ErrorResultApiModel>> VerifyOffersPriceAsync(OffersPriceApiModel model, string amaClientRef)
        {
            // Set the request properties
            var request = new AmadeusRequestApiModel<OffersPriceApiModel>
            {
                Endpoint = AmadeusEndpoints.VerifyOffersPrice,
                Headers = new Dictionary<string, string>
                {
                    { "ama-client-ref", amaClientRef }
                },
                Data = model,
                Method = HttpMethod.Post
            };

            // Trigger the request
            var response = await amadeusClient
                .SendAsync<OffersPriceApiModel, OffersPriceApiModel, WarningResultApiModel, ErrorResultApiModel>(request);

            // Return the response
            return response;
        }

        /// <summary>
        /// Configures and sends API request to create flight booking
        /// </summary>
        /// <param name="model">The flight search credentials</param>
        /// <returns></returns>
        public async Task<AmadeusApiResult<BookingResultApiModel, WarningResultApiModel, ErrorResultApiModel>> BookFlightAsync(FlightBookingApiModel model, string amaClientRef)
        {
            // Set the request properties
            var request = new AmadeusRequestApiModel<FlightBookingApiModel>
            {
                Endpoint = AmadeusEndpoints.BookFlightOffer,
                Headers = new Dictionary<string, string>
                {
                    { "ama-client-ref", amaClientRef }
                },
                Data = model,
                Method = HttpMethod.Post
            };

            // Trigger the request
            var response = await amadeusClient
                .SendAsync<FlightBookingApiModel, BookingResultApiModel, WarningResultApiModel, ErrorResultApiModel>(request);

            // Return the response
            return response;
        }
    }
}