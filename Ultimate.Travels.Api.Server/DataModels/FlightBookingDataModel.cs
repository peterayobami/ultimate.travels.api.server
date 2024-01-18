using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The bookings database table representational model
    /// </summary>
    public class FlightBookingDataModel : BaseDataModel
    {
        /// <summary>
        /// The customer id foreign key index
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// The PNR for this booking
        /// </summary>
        public string Pnr { get; set; }

        /// <summary>
        /// The order id for this booking
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// The ama client ref for this flight booking
        /// </summary>
        public string AmaClientRef { get; set; }

        /// <summary>
        /// The status of this booking
        /// </summary>
        public string BookingStatus { get; set; }

        /// <summary>
        /// The payment status of this booking
        /// </summary>
        public string PaymentStatus { get; set; }

        /// <summary>
        /// The customer related to this flight booking
        /// </summary>
        [ForeignKey(nameof(CustomerId))]
        public CustomersDataModel Customer { get; set; }

        /// <summary>
        /// The travelers entity relational model
        /// </summary>
        public List<TravelerDataModel> Travelers { get; set; }
    }
}