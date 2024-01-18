namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The flight requests database table representational model
    /// </summary>
    public class FlightRequestsDataModel : BaseDataModel
    {
        /// <summary>
        /// The id of customer that made this flight request
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// The JSON serialized search credentials for this flight request
        /// </summary>
        public string SearchCredentials { get; set; }

        /// <summary>
        /// The departure date of this flight request
        /// </summary>
        public DateTime DepartureDate { get; set; }
    }
}