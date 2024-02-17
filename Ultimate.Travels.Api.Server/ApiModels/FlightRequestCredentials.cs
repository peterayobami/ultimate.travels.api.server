using Swashbuckle.AspNetCore.Filters;

namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// Flight request credentials API model
    /// </summary>
    public class FlightRequestCredentials
    {
        /// <summary>
        /// The currency code for the fares
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// True, if flight must have zero connection
        /// </summary>
        public bool DirectFlight { get; set; }

        /// <summary>
        /// True, if offers dates must be flexible
        /// </summary>
        public bool FlexibleDates { get; set; }

        /// <summary>
        /// The trip's route model
        /// </summary>
        public RouteModel RouteModel { get; set; }

        /// <summary>
        /// The flight cabin for the trip
        /// </summary>
        public string FlightCabin { get; set; }

        /// <summary>
        /// The travelers specification
        /// </summary>
        public TravelersSpecification Travelers { get; set; }

        /// <summary>
        /// The one way route model specification
        /// </summary>
        public OneWayRouteModel OneWay { get; set; }

        /// <summary>
        /// The round trip route model specification
        /// </summary>
        public RoundTripRouteModel RoundTrip { get; set; }

        /// <summary>
        /// The multi city route model specification
        /// </summary>
        public MultiCityRouteModel MultiCity { get; set; }
    }

    /// <summary>
    /// Travelers specification model
    /// </summary>
    public class TravelersSpecification
    {
        /// <summary>
        /// The number of adults for the trip
        /// </summary>
        public int NumberOfAdults { get; set; }

        /// <summary>
        /// The number of children for the trip
        /// </summary>
        public int NumberOfChildren { get; set; }

        /// <summary>
        /// The number of infants for the trip
        /// </summary>
        public int NumberOfInfants { get; set; }
    }

    /// <summary>
    /// One way specification model
    /// </summary>
    public class OneWayRouteModel
    {
        /// <summary>
        /// The location of the traveler in form of the airport's iata code
        /// </summary>
        public string OriginLocation { get; set; }

        /// <summary>
        /// The destination of the traveler in form of the airport's iata code
        /// </summary>
        public string DestinationLocation { get; set; }

        /// <summary>
        /// The deprature date of the traveler, in yyyy-MM-dd format and must not be in the past
        /// </summary>
        public string DepartureDate { get; set; }
    }

    /// <summary>
    /// Round trip specification model
    /// </summary>
    public class RoundTripRouteModel : OneWayRouteModel
    {
        /// <summary>
        /// The return date of the traveler, in yyyy-MM-dd format and must not be before the departure date
        /// </summary>
        public string ReturnDate { get; set; }
    }

    /// <summary>
    /// Multi city routes specification model
    /// </summary>
    public class MultiCityRouteModel
    {
        /// <summary>
        /// The initial location of the traveler
        /// </summary>
        public string OriginLocation1 { get; set; }

        /// <summary>
        /// The initial destination of the traveler
        /// </summary>
        public string DestinationLocation1 { get; set; }

        /// <summary>
        /// The initial deprature date of the traveler
        /// </summary>
        public string DepartureDate1 { get; set; }

        /// <summary>
        /// The second location of the traveler
        /// </summary>
        public string OriginLocation2 { get; set; }

        /// <summary>
        /// The second destination of the traveler
        /// </summary>
        public string DestinationLocation2 { get; set; }

        /// <summary>
        /// The second deprature date of the traveler
        /// </summary>
        public string DepartureDate2 { get; set; }

        /// <summary>
        /// The third location of the traveler
        /// </summary>
        public string OriginLocation3 { get; set; }

        /// <summary>
        /// The third destination of the traveler
        /// </summary>
        public string DestinationLocation3 { get; set; }

        /// <summary>
        /// The third deprature date of the traveler
        /// </summary>
        public string DepartureDate3 { get; set; }

        /// <summary>
        /// The fourth location of the traveler
        /// </summary>
        public string OriginLocation4 { get; set; }

        /// <summary>
        /// The fourth destination of the traveler
        /// </summary>
        public string DestinationLocation4 { get; set; }

        /// <summary>
        /// The fourth deprature date of the traveler
        /// </summary>
        public string DepartureDate4 { get; set; }

        /// <summary>
        /// The fifth location of the traveler
        /// </summary>
        public string OriginLocation5 { get; set; }

        /// <summary>
        /// The fifth destination of the traveler
        /// </summary>
        public string DestinationLocation5 { get; set; }

        /// <summary>
        /// The fifth deprature date of the traveler
        /// </summary>
        public string DepartureDate5 { get; set; }
    }
}