namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// Handles operations relating to flight requests
    /// </summary>
    public static class FlightRequestUtility
    {
        /// <summary>
        /// Composes the flight search credentials based on specified request credentials
        /// </summary>
        /// <param name="requestCredentials">The specified request credentials</param>
        /// <returns></returns>
        public static OperationResult<FlightSearchApiModel> Compose(FlightRequestCredentials requestCredentials)
        {
            // Initialize flight search credentials
            var searchCredentials = new FlightSearchApiModel { CurrencyCode = requestCredentials.CurrencyCode ?? "USD" };

            // If one way was specified...
            if (requestCredentials.RouteModel == RouteModel.OneWay)
            {
                // Compose one way route
                var oneWayResult = ComposeOneWayOriginDestination(searchCredentials, requestCredentials);

                // If failed...
                if (!oneWayResult.Successful)
                    // Return error result
                    return new OperationResult<FlightSearchApiModel>
                    {
                        StatusCode = oneWayResult.StatusCode,
                        ErrorTitle = oneWayResult.ErrorTitle,
                        ErrorMessage = oneWayResult.ErrorMessage
                    };
            }

            // If round trip was specified...
            if (requestCredentials.RouteModel == RouteModel.RoundTrip)
            {
                // Compose round trip route
                var roundTripResult = ComposeRoundTripOriginDestination(searchCredentials, requestCredentials);

                // If failed...
                if (!roundTripResult.Successful)
                    // Return error result
                    return new OperationResult<FlightSearchApiModel>
                    {
                        StatusCode = roundTripResult.StatusCode,
                        ErrorTitle = roundTripResult.ErrorTitle,
                        ErrorMessage = roundTripResult.ErrorMessage
                    };
            }

            // If multi city was specified...
            if (requestCredentials.RouteModel == RouteModel.MultiCity)
            {
                // Compose multi city route
                var multiCityResult = ComposeMultiCityOriginDestination(searchCredentials, requestCredentials);

                // If failed...
                if (!multiCityResult.Successful)
                    // Return error result
                    return new OperationResult<FlightSearchApiModel>
                    {
                        StatusCode = multiCityResult.StatusCode,
                        ErrorTitle = multiCityResult.ErrorTitle,
                        ErrorMessage = multiCityResult.ErrorMessage
                    };
            }

            // Compose the travelers
            var travelersResult = ComposeTravelers(searchCredentials, requestCredentials);

            // If failed...
            if (!travelersResult.Successful)
            {
                // Return result
                return new OperationResult<FlightSearchApiModel>
                {
                    StatusCode = travelersResult.StatusCode,
                    ErrorTitle = travelersResult.ErrorTitle,
                    ErrorMessage = travelersResult.ErrorMessage
                };
            }

            // If flight cabin is not valid...
            if (!(requestCredentials.FlightCabin == FlightCabin.Economy || requestCredentials.FlightCabin == FlightCabin.PremiumEconomy ||
                requestCredentials.FlightCabin == FlightCabin.Business || requestCredentials.FlightCabin == FlightCabin.First))
            {
                // Return error result
                return new OperationResult<FlightSearchApiModel>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorTitle = "BAD REQUEST",
                    ErrorMessage = "You have provided an invalid flight cabin"
                };
            }

            // Set the sources
            searchCredentials.Sources = ["GDS"];

            // Get the origin/destination ids
            string[] originDestinationIds = searchCredentials.OriginDestinations.Select(o => o.Id).ToArray();

            // Set the search criteria
            searchCredentials.SearchCriteria = new SearchCriteria
            {
                MaxFlightOffers = 250,
                FlightFilters = new FlightFilters
                {
                    CabinRestrictions =
                    [
                        new() {
                            Cabin = requestCredentials.FlightCabin,
                            Coverage = "ALL_SEGMENTS",
                            OriginDestinationIds = originDestinationIds
                        }
                    ]
                }
            };

            // Return result
            return new OperationResult<FlightSearchApiModel>
            {
                StatusCode = StatusCodes.Status200OK,
                Result = searchCredentials
            };
        }

        /// <summary>
        /// Composes one way origin/destination route model
        /// </summary>
        /// <param name="searchCredentials">The provided search credentials</param>
        /// <param name="requestCredentials">The provided request credentials</param>
        /// <returns></returns>
        private static OperationResult ComposeOneWayOriginDestination(FlightSearchApiModel searchCredentials, FlightRequestCredentials requestCredentials)
        {
            if (string.IsNullOrEmpty(requestCredentials.OneWay.OriginLocation) || string.IsNullOrEmpty(requestCredentials.OneWay.DestinationLocation) || string.IsNullOrEmpty(requestCredentials.OneWay.DepartureDate))
            {
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorTitle = "BAD REQUEST",
                    ErrorMessage = "Please specify all relevant details for your locations and destinations"
                };
            }

            // Set the details for one way
            searchCredentials.OriginDestinations =
            [
                new() {
                    Id = "1",
                    OriginLocationCode = requestCredentials.OneWay.OriginLocation,
                    DestinationLocationCode = requestCredentials.OneWay.DestinationLocation,
                    DepartureDateTimeRange = new DepartureDateTimeRange
                    {
                        Date = requestCredentials.OneWay.DepartureDate
                    }
                }
            ];

            // Return result
            return new OperationResult { };
        }

        /// <summary>
        /// Composes round trip origin/destination route model
        /// </summary>
        /// <param name="searchCredentials">The provided search credentials</param>
        /// <param name="requestCredentials">The provided request credentials</param>
        /// <returns></returns>
        private static OperationResult ComposeRoundTripOriginDestination(FlightSearchApiModel searchCredentials, FlightRequestCredentials requestCredentials)
        {
            if (string.IsNullOrEmpty(requestCredentials.RoundTrip.OriginLocation) || string.IsNullOrEmpty(requestCredentials.RoundTrip.DestinationLocation) ||
                string.IsNullOrEmpty(requestCredentials.RoundTrip.DepartureDate) || string.IsNullOrEmpty(requestCredentials.RoundTrip.ReturnDate))
            {
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorTitle = "BAD REQUEST",
                    ErrorMessage = "Please specify all relevant details for your locations and destinations"
                };
            }

            // Set the details for round trip
            searchCredentials.OriginDestinations =
            [
                new() {
                    Id = "1",
                    OriginLocationCode = requestCredentials.RoundTrip.OriginLocation,
                    DestinationLocationCode = requestCredentials.RoundTrip.DestinationLocation,
                    DepartureDateTimeRange = new DepartureDateTimeRange
                    {
                        Date = requestCredentials.RoundTrip.DepartureDate
                    }
                },
                new() {
                    Id = "2",
                    OriginLocationCode = requestCredentials.RoundTrip.DestinationLocation,
                    DestinationLocationCode = requestCredentials.RoundTrip.OriginLocation,
                    DepartureDateTimeRange = new DepartureDateTimeRange
                    {
                        Date = requestCredentials.RoundTrip.ReturnDate
                    }
                }
            ];

            // Return result
            return new OperationResult { };
        }

        /// <summary>
        /// Composes multi city origin/destination route model
        /// </summary>
        /// <param name="searchCredentials">The provided search credentials</param>
        /// <param name="requestCredentials">The provided request credentials</param>
        /// <returns></returns>
        private static OperationResult ComposeMultiCityOriginDestination(FlightSearchApiModel searchCredentials, FlightRequestCredentials requestCredentials)
        {
            // Initialize origin destinations
            var originDestinations = new List<OriginDestination>();

            // For each origin/destinations...
            for (int i = 1; i <= 5; i++)
            {
                // Set the origin location
                string originLocation = (string)typeof(MultiCityRouteModel).GetProperty($"OriginLocation{i}").GetValue(requestCredentials.MultiCity);

                // Set the destination location
                string destinationLocation = (string)typeof(MultiCityRouteModel).GetProperty($"DestinationLocation{i}").GetValue(requestCredentials.MultiCity);

                // Set the departure date
                string departureDate = (string)typeof(MultiCityRouteModel).GetProperty($"DepartureDate{i}").GetValue(requestCredentials.MultiCity);

                // If any of the parameters were not specified...
                if (string.IsNullOrEmpty(originLocation) || string.IsNullOrEmpty(destinationLocation) || string.IsNullOrEmpty(departureDate))
                    // Terminate the iteration
                    break;

                // Create origin/destination details
                var originDestination = new OriginDestination
                {
                    Id = i.ToString(),
                    OriginLocationCode = originLocation,
                    DestinationLocationCode = destinationLocation,
                    DepartureDateTimeRange = new DepartureDateTimeRange
                    {
                        Date = departureDate
                    }
                };

                // Add it to the collection
                originDestinations.Add(originDestination);
            }

            // If we have less than two origin/destination...
            if (originDestinations.Count < 2)
                // Return error result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorTitle = "BAD REQUEST",
                    ErrorMessage = "Minimum of two origins and destinations are required for the multi city route model"
                };

            // Set the origin destinations
            searchCredentials.OriginDestinations = originDestinations;

            // Return result
            return new OperationResult { };
        }

        /// <summary>
        /// Composes travelers object model
        /// </summary>
        /// <param name="searchCredentials">The provided search credentials</param>
        /// <param name="requestCredentials">The provided request credentials</param>
        /// <returns></returns>
        private static OperationResult ComposeTravelers(FlightSearchApiModel searchCredentials, FlightRequestCredentials requestCredentials)
        {
            // If we don't have at least one adult traveler...
            if (requestCredentials.Travelers.NumberOfAdults < 1)
            {
                // Return error result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorTitle = "BAD REQUEST",
                    ErrorMessage = "At least one adult is required for a trip"
                };
            }

            // If we have more infants than adults...
            if (requestCredentials.Travelers.NumberOfAdults < requestCredentials.Travelers.NumberOfInfants)
            {
                // Return error result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorTitle = "BAD REQUEST",
                    ErrorMessage = "Number of infants cannot be greater than number of adults"
                };
            }

            // Initialize travelers
            var travelers = new List<Traveler> { };

            // For each adult traveler...
            for (int i = 1; i <= requestCredentials.Travelers.NumberOfAdults; i++)
            {
                // Create adult traveler
                var traveler = new Traveler
                {
                    Id = i.ToString(),
                    TravelerType = TravelerType.Adult,
                    FareOptions = ["STANDARD"]
                };

                // Add the traveler to the collection
                travelers.Add(traveler);
            }

            // For each child traveler...
            for (int i = 1; i <= requestCredentials.Travelers.NumberOfChildren; i++)
            {
                // Set the traveler index
                int travelerIndex = travelers.Count + 1;

                // Create adult traveler
                var traveler = new Traveler
                {
                    Id = travelerIndex.ToString(),
                    TravelerType = TravelerType.Child,
                    FareOptions = ["STANDARD"]
                };

                // Add the traveler to the collection
                travelers.Add(traveler);
            }

            // For each infant traveler...
            for (int i = 1; i <= requestCredentials.Travelers.NumberOfInfants; i++)
            {
                // Set the traveler index
                int travelerIndex = travelers.Count + 1;

                // Create adult traveler
                var traveler = new Traveler
                {
                    Id = travelerIndex.ToString(),
                    AssociatedAdultId = i.ToString(),
                    TravelerType = TravelerType.HeldInfant,
                    FareOptions = ["STANDARD"]
                };

                // Add the traveler to the collection
                travelers.Add(traveler);
            }

            // Set the travelers
            searchCredentials.Travelers = travelers;

            // Return result
            return new OperationResult { };
        }
    }
}