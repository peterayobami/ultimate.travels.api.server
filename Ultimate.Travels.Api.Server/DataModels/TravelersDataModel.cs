using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The travelers database table representational model
    /// </summary>
    public class TravelerDataModel : BaseDataModel
    {
        /// <summary>
        /// The flight booking id relational model
        /// </summary>
        public string FlightBookingId { get; set; }

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
        /// The middle name of the traveler
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// The gender of the traveler
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// The id of the traveler according to the sequential order
        /// </summary>
        public string TravelerId { get; set; }

        /// <summary>
        /// The type of the traveler
        /// </summary>
        public string TravelerType { get; set; }

        /// <summary>
        /// The date of birth of the traveler
        /// </summary>
        public string DateOfBirth { get; set; }

        /// <summary>
        /// The flight booking relational model
        /// </summary>
        [ForeignKey(nameof(FlightBookingId))]
        public FlightBookingDataModel FlightBooking { get; set; }
    }
}