namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The traveler's credentials
    /// </summary>
    public class TravelerCredentials
    {
        /// <summary>
        /// The id of the traveler accourding to the sequential order of traveler pricings
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The type of the traveler
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The title of the traveler
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The first name of the traveler
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the traveler
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The gender of the traveler
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// The date of birth of the traveler
        /// </summary>
        public DateTime DateOfBirth { get; set; }
    }

    /// <summary>
    /// The traveler contact information
    /// </summary>
    public class TravelerContact
    {
        /// <summary>
        /// The email of the traveler
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The phone number of the traveler
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// The country dialing code of the traveler's contact
        /// </summary>
        public string CountryDialingCode { get; set; }

        /// <summary>
        /// The first name of the traveler contact
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the traveler contact
        /// </summary>
        public string LastName { get; set; }
    }
}
