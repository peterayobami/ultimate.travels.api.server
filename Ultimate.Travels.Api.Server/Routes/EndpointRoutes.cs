namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The endpoint routes constants
    /// </summary>
    public class EndpointRoutes
    {
        /// <summary>
        /// The route to the PersistFlightRequest endpoint
        /// </summary>
        public const string PersistFlightRequest = "api/flight-request/persist";
        
        /// <summary>
        /// The route to the FetchFlightOffers endpoint
        /// </summary>
        public const string FetchFlightOffers = "api/flight-offers/fetch";
        
        /// <summary>
        /// The route to the VerifyOffersPricing endpoint
        /// </summary>
        public const string VerifyOffersPricing = "api/flight-offers-pricing/verify";
        
        /// <summary>
        /// The route to the InitializeFlightBooking endpoint
        /// </summary>
        public const string InitializeFlightBooking = "api/flight-booking/initialize";
        
        /// <summary>
        /// The route to the CreateFlightBooking endpoint
        /// </summary>
        public const string CreateFlightBooking = "api/flight-booking/create";

        /// <summary>
        /// The route to the FetchFlightBooking endpoint
        /// </summary>
        public const string FetchFlightBooking = "api/flight-booking/fetch";

        /// <summary>
        /// The route to the InitializePayStackPayment endpoint
        /// </summary>
        public const string InitializePayStackPayment = "api/paystack-payment/initialize";
        
        /// <summary>
        /// The route to the VerifyPayStackPayment endpoint
        /// </summary>
        public const string VerifyPayStackPayment = "api/paystack-payment/verify";
    }
}