namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The cron expression constants
    /// </summary>
    public class CronExpression
    {
        /// <summary>
        /// The cron expression for authorization credentials job
        /// </summary>
        public const string UpdateAuthorizationCredentials = "*/20 * * * *";

        /// <summary>
        /// The cron expression for the flight requests manager job
        /// </summary>
        public const string FlightRequestsManager = "0 */23 * * *";

        /// <summary>
        /// The cron expression for the flight offers manager job
        /// </summary>
        public const string FlightOffersManager = "0 */1 * * *";
    }
}