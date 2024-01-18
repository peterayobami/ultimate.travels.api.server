namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The result of a flight search transaction
    /// </summary>
    public class FlightResultApiModel
    {
        public MetaApiModel Meta { get; set; }
        public List<FlightOfferDetail> FlightOffers { get; set; }
        public DictionariesApiModel Dictionaries { get; set; }
    }

    public class FlightOfferDetail
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Source { get; set; }
        public string AmaClientRef { get; set; }
        public bool InstantTicketingRequired { get; set; }
        public bool NonHomogeneous { get; set; }
        public bool OneWay { get; set; }
        public string LastTicketingDate { get; set; }
        public int NumberOfBookableSeats { get; set; }
        public int DurationInMinutes { get; set; }
        public List<ItineraryDetail> Itineraries { get; set; }
        public PriceApiModel Price { get; set; }
        public PricingOptionsApiModel PricingOptions { get; set; }
        public string[] ValidatingAirlineCodes { get; set; }
        public List<TravelerPricingApiModel> TravelerPricings { get; set; }
    }

    public class ItineraryDetail
    {
        public string Duration { get; set; }
        public int DurationInMinutes { get; set; }
        public List<SegmentDetail> Segments { get; set; }
    }

    public class SegmentDetail
    {
        public SegmentDeparture Departure { get; set; }
        public SegmentArrival Arrival { get; set; }
        public AirlineApiModel Carrier { get; set; }
        public string Number { get; set; }
        public AircraftApiModel Aircraft { get; set; }
        public OperatingCredential Operating { get; set; }
        public string Duration { get; set; }
        public string Id { get; set; }
        public int NumberOfStops { get; set; }
        public bool BlacklistedInEU { get; set; }
    }

    public class SegmentDeparture
    {
        public AirportApiModel Location { get; set; }
        public DateTime At { get; set; }
    }

    public class SegmentArrival
    {
        public AirportApiModel Location { get; set; }
        public DateTime At { get; set; }
    }

    public class OperatingCredential
    {
        public AirlineApiModel Carrier { get; set; }
    }
}