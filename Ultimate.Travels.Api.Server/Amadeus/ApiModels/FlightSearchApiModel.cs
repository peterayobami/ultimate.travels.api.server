namespace Ultimate.Travels.Api.Server
{
    public class FlightSearchApiModel
    {
        public string CurrencyCode { get; set; }
        public List<OriginDestination> OriginDestinations { get; set; }
        public List<Traveler> Travelers { get; set; }
        public List<string> Sources { get; set; }
        public SearchCriteria SearchCriteria { get; set; }
    }

    public class DepartureDateTimeRange
    {
        public string Date { get; set; }
        public string Time { get; set; }
    }

    public class OriginDestination
    {
        public string Id { get; set; }
        public string OriginLocationCode { get; set; }
        public string DestinationLocationCode { get; set; }
        public DepartureDateTimeRange DepartureDateTimeRange { get; set; }
    }

    public class Traveler
    {
        public string Id { get; set; }
        public string AssociatedAdultId { get; set; }
        public string TravelerType { get; set; }
        public string[] FareOptions { get; set; }
    }

    public class CabinRestrictions
    {
        public string Cabin { get; set; }
        public string Coverage { get; set; }
        public string[] OriginDestinationIds { get; set; }
    }

    public class CarrierRestrictions
    {
        public string[] IncludedCarrierCodes { get; set; }
        public string[] ExcludedCarrierCodes { get; set; }
    }

    public class FlightFilters
    {
        public CabinRestrictions[] CabinRestrictions { get; set; }
        public CarrierRestrictions CarrierRestrictions { get; set; }
    }

    public class SearchCriteria
    {
        public int MaxFlightOffers { get; set; }
        public FlightFilters FlightFilters { get; set; }
    }
}