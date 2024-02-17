using Swashbuckle.AspNetCore.Filters;

namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// Flight request credential examples
    /// </summary>
    public class FlightRequestCredentialsModelExample : IExamplesProvider<FlightRequestCredentials>
    {
        /// <summary>
        /// Gets the sample value for object properties
        /// </summary>
        /// <returns></returns>
        public FlightRequestCredentials GetExamples()
        {
            return new FlightRequestCredentials
            {
                CurrencyCode = "USD",
                DirectFlight = false,
                FlexibleDates = false,
                RouteModel = RouteModel.OneWay,
                FlightCabin = FlightCabin.Economy,
                Travelers = new TravelersSpecification
                {
                    NumberOfAdults = 1,
                    NumberOfChildren = 0,
                    NumberOfInfants = 0
                },
                OneWay = new OneWayRouteModel
                {
                    OriginLocation = "LOS",
                    DestinationLocation = "LHR",
                    DepartureDate = "2024-02-27"
                }
            };
        }
    }
}
