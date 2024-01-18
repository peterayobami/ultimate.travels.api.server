namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The flight booking result API model
    /// </summary>
    public class BookingResultApiModel
    {
        public List<Warning> Warnings { get; set; }
        public Datum Data { get; set; }
        public DictionariesApiModel Dictionaries { get; set; }

        public class Warning
        {
            public int Status { get; set; }
            public int Code { get; set; }
            public string Title { get; set; }
            public string Detail { get; set; }
        }

        public class Datum
        {
            public string Type { get; set; }
            public string Id { get; set; }
            public string QueuingOfficeId { get; set; }
            public List<AssociatedRecord> AssociatedRecords { get; set; }
            public List<FlightDataApiModel> FlightOffers { get; set; }
            public List<FlightBookingApiModel.TravelerApiModel> Travelers { get; set; }
            public FlightBookingApiModel.Remarks Remarks { get; set; }
            public FlightBookingApiModel.TicketingAgreement TicketingAgreement { get; set; }
            public List<AutomatedProcessItem> AutomatedProcess { get; set; }
            public List<FlightBookingApiModel.ContactInfo> Contact { get; set; }
        }

        public class Queue
        {
            public string Number { get; set; }
            public string Category { get; set; }
        }

        public class AutomatedProcessItem
        {
            public string Code { get; set; }
            public Queue Queue { get; set; }
            public string OfficeId { get; set; }
        }

        public class AssociatedRecord
        {
            public string Reference { get; set; }
            public DateTime CreationDate { get; set; }
            public string OriginSystemCode { get; set; }
            public string FlightOfferId { get; set; }
        }
    }
}
