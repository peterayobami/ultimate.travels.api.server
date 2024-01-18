namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// Handles itinerary related operations
    /// </summary>
    public static class FlightItineraryUtility
    {
        /// <summary>
        /// Composes itineraries to a detailed itinerary information
        /// </summary>
        /// <param name="itineraries">The specified itineraries</param>
        /// <param name="airports">The provided airports data</param>
        /// <param name="airlines">The provided airlines data</param>
        /// <returns></returns>
        public static List<ItineraryDetail> Compose(List<ItineraryApiModel> itineraries, List<AirportApiModel> airports, List<AirlineApiModel> airlines)
        {
            // Initialize itineraries
            List<ItineraryDetail> itineraryDetails = [];

            // For each itinerary...
            for (int i = 0; i < itineraries.Count; i++)
            {
                // Get the itinerary
                var itinerary = itineraries[i];

                // Initialize segments
                List<SegmentDetail> segmentDetails = [];

                // For each segment...
                for (int index = 0; index < itinerary.Segments.Count; index++)
                {
                    // Get the segment
                    var segment = itinerary.Segments[index];

                    // Create the segment detail
                    var segmentDetail = new SegmentDetail
                    {
                        Departure = new SegmentDeparture
                        {
                            At = segment.Departure.At,
                            Location = airports.FirstOrDefault(a => a.IataCode == segment.Departure.IataCode)
                        },
                        Arrival = new SegmentArrival
                        {
                            At = segment.Arrival.At,
                            Location = airports.FirstOrDefault(a => a.IataCode == segment.Arrival.IataCode)
                        },
                        Carrier = airlines.FirstOrDefault(a => a.IataCode == segment.CarrierCode),
                        Number = segment.Number,
                        Aircraft = segment.Aircraft,
                        Operating = segment.Operating != null ? new OperatingCredential
                        {
                            Carrier = airlines.FirstOrDefault(a => a.IataCode == segment.Operating.CarrierCode)
                        } : null,
                        Duration = segment.Duration.ConvertFromIso8601(),
                        Id = segment.Id,
                        NumberOfStops = segment.NumberOfStops,
                        BlacklistedInEU = segment.BlacklistedInEU
                    };

                    // Add segment to collection
                    segmentDetails.Add(segmentDetail);
                }

                // Create the itinerary detail
                var itineraryDetail = new ItineraryDetail
                {
                    Duration = itinerary.Duration.ConvertFromIso8601(),
                    DurationInMinutes = itinerary.Duration.GetDurationInMinutes(),
                    Segments = segmentDetails
                };

                // Add itinerary to collection
                itineraryDetails.Add(itineraryDetail);
            }

            // Return itinerary details
            return itineraryDetails;
        }

        /// <summary>
        /// Decomposes itineraries detaill to the simple and plain ItineraryApiModel
        /// </summary>
        /// <param name="itineraryDetails">The provided itinerary detail</param>
        /// <returns></returns>
        public static List<ItineraryApiModel> Decompose(List<ItineraryDetail> itineraryDetails)
        {
            // Initialize itineraries
            List<ItineraryApiModel> itineraries = [];

            // For eachc itinerary detail...
            for (int i = 0; i < itineraryDetails.Count; i++)
            {
                // Get the itinerary detail
                var itineraryDetail = itineraryDetails[i];

                // Initialize segments
                List<SegmentApiModel> segments = [];

                // For each segment...
                for (int index = 0; index < itineraryDetail.Segments.Count; index++)
                {
                    // Set the segment
                    var segmentDetail = itineraryDetail.Segments[index];

                    // Create segment
                    var segment = new SegmentApiModel
                    {
                        Departure = new DepartureApiModel
                        {
                            At = segmentDetail.Departure.At,
                            IataCode = segmentDetail.Departure.Location.IataCode
                        },
                        Arrival = new ArrivalApiModel
                        {
                            At = segmentDetail.Arrival.At,
                            IataCode = segmentDetail.Arrival.Location.IataCode
                        },
                        CarrierCode = segmentDetail.Carrier.IataCode,
                        Number = segmentDetail.Number,
                        Aircraft = segmentDetail.Aircraft,
                        Operating = new OperatingApiModel
                        {
                            CarrierCode = segmentDetail.Operating.Carrier.IataCode
                        },
                        Duration = segmentDetail.Duration.ConvertToIso8601(),
                        Id = segmentDetail.Id,
                        NumberOfStops = segmentDetail.NumberOfStops,
                        BlacklistedInEU = segmentDetail.BlacklistedInEU
                    };

                    // Add segment to collection
                    segments.Add(segment);
                }

                // Create the itinerary
                var itinerary = new ItineraryApiModel
                {
                    Duration = itineraryDetail.Duration.ConvertToIso8601(),
                    Segments = segments
                };

                // Add itinerary to collection
                itineraries.Add(itinerary);
            }

            // Return result
            return itineraries;
        }
    }
}