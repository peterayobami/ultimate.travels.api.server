namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The customers database table representational model
    /// </summary>
    public class CustomersDataModel : BaseDataModel
    {
        /// <summary>
        /// The email address of the customer
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The phone number of the customer
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// The customer's contact country dialing code
        /// </summary>
        public string CountryDialingCode { get; set; }

        /// <summary>
        /// The first name of the customer
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the customer
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The flight booking related to this customer
        /// </summary>
        public List<FlightBookingDataModel> FlightBookings { get; set; }
    }
}