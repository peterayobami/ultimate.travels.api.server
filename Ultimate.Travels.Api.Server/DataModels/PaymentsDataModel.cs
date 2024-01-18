using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The payments database table representational model
    /// </summary>
    public class PaymentsDataModel : BaseDataModel
    {
        /// <summary>
        /// The payment reference id
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// The flight booking id foreign key index
        /// </summary>
        public string FlightBookingId { get; set; }

        /// <summary>
        /// The payment status
        /// TODO: Add constraints for allowed values
        /// </summary>
        public string PaymentStatus { get; set; }

        /// <summary>
        /// The flight booking relational model
        /// </summary>
        [ForeignKey(nameof(FlightBookingId))]
        public FlightBookingDataModel FlightBooking { get; set; }
    }
}