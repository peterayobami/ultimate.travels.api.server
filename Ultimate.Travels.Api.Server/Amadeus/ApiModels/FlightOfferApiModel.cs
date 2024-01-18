namespace Ultimate.Travels.Api.Server
{
    public class FlightOfferApiModel
    {
        public MetaApiModel Meta { get; set; }
        public List<FlightDataApiModel> Data { get; set; }
        public DictionariesApiModel Dictionaries { get; set; }
    }

    public class MetaApiModel
    {
        public int Count { get; set; }
    }

    public class FlightDataApiModel
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Source { get; set; }
        public bool InstantTicketingRequired { get; set; }
        public bool NonHomogeneous { get; set; }
        public bool OneWay { get; set; }
        public string LastTicketingDate { get; set; }
        public string LastTicketingDateTime { get; set; }
        public int NumberOfBookableSeats { get; set; }
        public List<ItineraryApiModel> Itineraries { get; set; }
        public PriceApiModel Price { get; set; }
        public PricingOptionsApiModel PricingOptions { get; set; }
        public string[] ValidatingAirlineCodes { get; set; }
        public List<TravelerPricingApiModel> TravelerPricings { get; set; }
    }

    public class ItineraryApiModel
    {
        public string Duration { get; set; }
        public List<SegmentApiModel> Segments { get; set; }
    }

    public class SegmentApiModel
    {
        public DepartureApiModel Departure { get; set; }
        public ArrivalApiModel Arrival { get; set; }
        public string CarrierCode { get; set; }
        public string Number { get; set; }
        public AircraftApiModel Aircraft { get; set; }
        public OperatingApiModel Operating { get; set; }
        public string Duration { get; set; }
        public string Id { get; set; }
        public int NumberOfStops { get; set; }
        public bool BlacklistedInEU { get; set; }
    }

    public class DepartureApiModel
    {
        public string IataCode { get; set; }
        public string Terminal { get; set; }
        public DateTime At { get; set; }
    }

    public class ArrivalApiModel
    {
        public string IataCode { get; set; }
        public string Terminal { get; set; }
        public DateTime At { get; set; }
    }

    public class AircraftApiModel
    {
        public string Code { get; set; }
    }

    public class OperatingApiModel
    {
        public string CarrierCode { get; set; }
    }

    public class TravelerPrice
    {
        public string Currency { get; set; }
        public string Total { get; set; }
        public string Base { get; set; }
    }

    public class PriceApiModel
    {
        public string Currency { get; set; }
        public string Total { get; set; }
        public string Base { get; set; }
        public List<FeeApiModel> Fees { get; set; }
        public string GrandTotal { get; set; }
        public string BillingCurrency { get; set; }
        public List<AdditionalService> AdditionalServices { get; set; }
    }

    public class AdditionalService
    {
        public string Amount { get; set; }
        public string Type { get; set; }
    }

    public class FeeApiModel
    {
        public string Amount { get; set; }
        public string Type { get; set; }
    }

    public class PricingOptionsApiModel
    {
        public string[] FareType { get; set; }
        public bool IncludedCheckedBagsOnly { get; set; }
    }

    public class TravelerPricingApiModel
    {
        public string TravelerId { get; set; }
        public string FareOption { get; set; }
        public string TravelerType { get; set; }
        public TravelerPrice Price { get; set; }
        public List<FareDetailsBySegmentApiModel> FareDetailsBySegment { get; set; }
    }

    public class FareDetailsBySegmentApiModel
    {
        public string SegmentId { get; set; }
        public string Cabin { get; set; }
        public string FareBasis { get; set; }
        public string BrandedFare { get; set; }
        public string BrandedFareLabel { get; set; }
        public string Class { get; set; }
        public IncludedCheckedBagsApiModel IncludedCheckedBags { get; set; }
        public List<Amenity> Amenities { get; set; }
    }

    public class AmenityProvider
    {
        public string Name { get; set; }
    }

    public class Amenity
    {
        public string Description { get; set; }
        public bool IsChargeable { get; set; }
        public string AmenityType { get; set; }
        public AmenityProvider AmenityProvider { get; set; }
    }

    public class IncludedCheckedBagsApiModel
    {
        public int Quantity { get; set; }
    }

    public class DictionariesApiModel
    {
        public Dictionary<string, LocationApiModel> Locations { get; set; }
        public Dictionary<string, string> Aircraft { get; set; }
        public Dictionary<string, string> Currencies { get; set; }
        public Dictionary<string, string> Carriers { get; set; }
    }

    public class LocationApiModel
    {
        public string CityCode { get; set; }
        public string CountryCode { get; set; }
    }
}