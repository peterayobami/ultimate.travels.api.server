namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The booking status constants
    /// </summary>
    public class BookingStatus
    {
        /// <summary>
        /// States that booking has been initialized
        /// but not processed further
        /// </summary>
        public const string Initialized = "INITIALIZED";

        /// <summary>
        /// States that booking have failed
        /// </summary>
        public const string Failed = "FAILED";

        /// <summary>
        /// States that booking is pending
        /// </summary>
        public const string Pending = "PENDING";

        /// <summary>
        /// States that booking have been confirmed
        /// </summary>
        public const string Confirmed = "CONFIRMED";
    }
}