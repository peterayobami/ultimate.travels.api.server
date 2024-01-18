namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The flight offers database table representational model
    /// </summary>
    public class FlightOfferDataModel : BaseDataModel
    {
        /// <summary>
        /// The ama client ref for this flight offer
        /// </summary>
        public string AmaClientRef { get; set; }

        /// <summary>
        /// The flight offer for this record
        /// </summary>
        public string Data { get; set; }
    }
}
