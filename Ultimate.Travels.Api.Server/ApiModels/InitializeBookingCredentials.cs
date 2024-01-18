namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The credentials to initialize flight booking
    /// </summary>
    public class InitializeBookingCredentials
    {
        /// <summary>
        /// The ama client ref of the selected flight offer
        /// </summary>
        public string AmaClientRef { get; set; }

        /// <summary>
        /// The traveler credentials for this booking
        /// </summary>
        public List<TravelerCredentials> TravelerCredentials { get; set; }

        /// <summary>
        /// The traveler(s) contact for this booking
        /// </summary>
        public TravelerContact TravelerContact { get; set; }
    }
}
