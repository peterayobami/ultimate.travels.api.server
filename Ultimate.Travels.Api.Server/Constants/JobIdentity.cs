namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The background job constants
    /// </summary>
    public class JobIdentity
    {
        /// <summary>
        /// The id of background job that updates authorization credentials
        /// </summary>
        public const string UpdateAuthorizationCredentials = "ee294351-41fa-4c17-9e89-67a02f7a3023";

        /// <summary>
        /// The id of backgrounf job that manages flight requests
        /// </summary>
        public const string FlightRequestsManager = "c3171e2a-d427-4c53-bbd9-35f659c38bea";

        /// <summary>
        /// The id of backgroung job that manages flight offers
        /// </summary>
        public const string FlightOffersManager = "e5e5aa43-72c3-42c0-b87a-b63f2b666bc3";
    }
}