using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The shopping operations domain service
    /// </summary>
    public class ShoppingOperations
    {
        #region Private Members

        /// <summary>
        /// The scoped instance of the <see cref="ApplicationDbContext"/>
        /// </summary>
        private readonly ApplicationDbContext context;

        /// <summary>
        /// The singleton instance of the <see cref="ILogger"/>
        /// </summary>
        private readonly ILogger<ShoppingOperations> logger;

        /// <summary>
        /// The scoped instance of the <see cref="AmadeusService"/>
        /// </summary>
        private readonly AmadeusService amadeusService;

        #endregion

        #region Constructor

        public ShoppingOperations(ApplicationDbContext context, ILogger<ShoppingOperations> logger,
            AmadeusService amadeusService)
        {
            this.context = context;
            this.logger = logger;
            this.amadeusService = amadeusService;
        }

        #endregion

        /// <summary>
        /// Compose and store specified credentials
        /// </summary>
        /// <param name="requestCredentials">The specified flight request credentials</param>
        /// <returns>Return flight request id for the persisted credentials</returns>
        public async Task<OperationResult> PersistRequestAsync(FlightRequestCredentials requestCredentials, string customerId = null)
        {
            try
            {
                // Compose flight request
                var composeResult = FlightRequestUtility.Compose(requestCredentials);

                // If composition failed...
                if (!composeResult.Successful)
                {
                    // Return error response
                    return new OperationResult
                    {
                        StatusCode = composeResult.StatusCode,
                        ErrorTitle = composeResult.ErrorTitle,
                        ErrorMessage = composeResult.ErrorMessage
                    };
                }

                // Set the flight request details
                var flightRequest = new FlightRequestsDataModel
                {
                    CustomerId = customerId,
                    SearchCredentials = JsonSerializer.Serialize(composeResult.Result),
                    DepartureDate = DateTime.Parse(composeResult.Result.OriginDestinations[0].DepartureDateTimeRange.Date).ToUniversalTime()
                };

                // Create the flight request
                await context.FlightRequests.AddAsync(flightRequest);

                // Save changes
                await context.SaveChangesAsync();

                // Return result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status201Created,
                    Result = new { requestId = flightRequest.Id }
                };
            }
            catch (Exception ex)
            {
                // Log exception
                logger.LogError(ex.Message);

                // Return error result
                return new OperationResult
                {
                    ErrorTitle = "SYSTEM ERROR",
                    ErrorMessage = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Fetches and processes flight offers
        /// </summary>
        /// <param name="requestId">The specified flight request id</param>
        /// <returns></returns>
        public async Task<OperationResult> FetchOffersAsync(string requestId)
        {
            try
            {
                // Retrieve flight request
                var flightRequest = await context.FlightRequests.FirstOrDefaultAsync(x => x.Id == requestId);

                // If flight request was not found...
                if (flightRequest == null)
                {
                    // Log error message
                    logger.LogError("Flight request not found");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "NOT FOUND",
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Flight request not found"
                    };
                }

                // Deserialize search credentials
                var searchCredentials = JsonSerializer.Deserialize<FlightSearchApiModel>(flightRequest.SearchCredentials);

                // Generate ama client ref
                var amaClientRef = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

                // Trigger the request
                var searchResponse = await amadeusService.GetOffersAsync(searchCredentials, amaClientRef);

                // If request failed...
                if (!searchResponse.Successful)
                {
                    // Log error
                    logger.LogError(searchResponse.ErrorMessage);

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = searchResponse.ErrorResult != null ? searchResponse.ErrorResult.Errors[0].Title : "SYSTEM ERROR",
                        StatusCode = searchResponse.ErrorResult != null ? searchResponse.ErrorResult.Errors[0].Status : StatusCodes.Status500InternalServerError,
                        ErrorMessage = searchResponse.ErrorResult != null ? searchResponse.ErrorResult.Errors[0].Detail : "An unknown error has occurred",
                        ErrorResult = searchResponse.ErrorResult
                    };
                }

                // Create JSON options
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                // Get airlines
                var jsonAirlines = File.ReadAllText("Assets/airlines.json");
                var airlines = JsonSerializer.Deserialize<List<AirlineApiModel>>(jsonAirlines, jsonOptions);

                // Get airports
                var jsonAirports = File.ReadAllText("Assets/airports.json");
                var airports = JsonSerializer.Deserialize<List<AirportApiModel>>(jsonAirports, jsonOptions);

                // Initialize flight offers
                List<FlightOfferDetail> flightOffers = [];

                // Set the offers
                var offers = searchResponse.Result.Data;

                // For each flight offer...
                for (int i = 0; i < offers.Count; i++)
                {
                    // Set offer
                    var offer = offers[i];

                    // Compose itineraries
                    var itineraries = FlightItineraryUtility.Compose(offer.Itineraries, airports, airlines);

                    // Create offer detail
                    var offerDetail = new FlightOfferDetail
                    {
                        Type = offer.Type,
                        Id = offer.Id,
                        Source = offer.Source,
                        AmaClientRef = amaClientRef,
                        InstantTicketingRequired = offer.InstantTicketingRequired,
                        NonHomogeneous = offer.NonHomogeneous,
                        OneWay = offer.OneWay,
                        LastTicketingDate = offer.LastTicketingDate,
                        NumberOfBookableSeats = offer.NumberOfBookableSeats,
                        DurationInMinutes = itineraries.Sum(d => d.DurationInMinutes),
                        Itineraries = itineraries,
                        Price = offer.Price,
                        PricingOptions = offer.PricingOptions,
                        ValidatingAirlineCodes = offer.ValidatingAirlineCodes,
                        TravelerPricings = offer.TravelerPricings
                    };

                    // Add offer to collection
                    flightOffers.Add(offerDetail);
                }

                // Return result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status200OK,
                    Result = new FlightResultApiModel
                    {
                        Meta = searchResponse.Result.Meta,
                        FlightOffers = flightOffers,
                        Dictionaries = searchResponse.Result.Dictionaries
                    }
                };
            }
            catch (Exception ex)
            {
                // Log error
                logger.LogError(ex.Message);
                logger.LogError(ex.StackTrace);

                // Return error result
                return new OperationResult
                {
                    ErrorTitle = "SYSTEM ERROR",
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Fetches and processes flight offers
        /// </summary>
        /// <param name="requestId">The specified flight request id</param>
        /// <returns></returns>
        public async Task<OperationResult> FetchOffersAsync(FlightRequestCredentials requestCredentials)
        {
            try
            {
                // Compose flight request
                var composeResult = FlightRequestUtility.Compose(requestCredentials);

                // If composition failed...
                if (!composeResult.Successful)
                {
                    // Return error response
                    return new OperationResult
                    {
                        StatusCode = composeResult.StatusCode,
                        ErrorTitle = composeResult.ErrorTitle,
                        ErrorMessage = composeResult.ErrorMessage
                    };
                }

                // Deserialize search credentials
                var searchCredentials = composeResult.Result;

                // Generate ama client ref
                var amaClientRef = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

                // Trigger the request
                var searchResponse = await amadeusService.GetOffersAsync(searchCredentials, amaClientRef);

                // If request failed...
                if (!searchResponse.Successful)
                {
                    // Log error
                    logger.LogError(searchResponse.ErrorMessage);

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = searchResponse.ErrorResult != null ? searchResponse.ErrorResult.Errors[0].Title : "SYSTEM ERROR",
                        StatusCode = searchResponse.ErrorResult != null ? searchResponse.ErrorResult.Errors[0].Status : StatusCodes.Status500InternalServerError,
                        ErrorMessage = searchResponse.ErrorResult != null ? searchResponse.ErrorResult.Errors[0].Detail : "An unknown error has occurred",
                        ErrorResult = searchResponse.ErrorResult
                    };
                }

                // Create JSON options
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                // Get airlines
                var jsonAirlines = File.ReadAllText("Assets/airlines.json");
                var airlines = JsonSerializer.Deserialize<List<AirlineApiModel>>(jsonAirlines, jsonOptions);

                // Get airports
                var jsonAirports = File.ReadAllText("Assets/airports.json");
                var airports = JsonSerializer.Deserialize<List<AirportApiModel>>(jsonAirports, jsonOptions);

                // Initialize flight offers
                List<FlightOfferDetail> flightOffers = [];

                // Set the offers
                var offers = searchResponse.Result.Data;

                // For each flight offer...
                for (int i = 0; i < offers.Count; i++)
                {
                    // Set offer
                    var offer = offers[i];

                    // Compose itineraries
                    var itineraries = FlightItineraryUtility.Compose(offer.Itineraries, airports, airlines);

                    // Create offer detail
                    var offerDetail = new FlightOfferDetail
                    {
                        Type = offer.Type,
                        Id = offer.Id,
                        Source = offer.Source,
                        AmaClientRef = amaClientRef,
                        InstantTicketingRequired = offer.InstantTicketingRequired,
                        NonHomogeneous = offer.NonHomogeneous,
                        OneWay = offer.OneWay,
                        LastTicketingDate = offer.LastTicketingDate,
                        NumberOfBookableSeats = offer.NumberOfBookableSeats,
                        DurationInMinutes = itineraries.Sum(d => d.DurationInMinutes),
                        Itineraries = itineraries,
                        Price = offer.Price,
                        PricingOptions = offer.PricingOptions,
                        ValidatingAirlineCodes = offer.ValidatingAirlineCodes,
                        TravelerPricings = offer.TravelerPricings
                    };

                    // Add offer to collection
                    flightOffers.Add(offerDetail);
                }

                // Return result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status200OK,
                    Result = new FlightResultApiModel
                    {
                        Meta = searchResponse.Result.Meta,
                        FlightOffers = flightOffers,
                        Dictionaries = searchResponse.Result.Dictionaries
                    }
                };
            }
            catch (Exception ex)
            {
                // Log error
                logger.LogError(ex.Message);
                logger.LogError(ex.StackTrace);

                // Return error result
                return new OperationResult
                {
                    ErrorTitle = "SYSTEM ERROR",
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Verify the price of specified offer detail
        /// </summary>
        /// <param name="offerDetail">The specified offer detail</param>
        /// <returns></returns>
        public async Task<OperationResult> VerifyOffersPriceAsync(FlightOfferDetail offerDetail)
        {
            try
            {
                // Decompose itineraries
                var itineraries = FlightItineraryUtility.Decompose(offerDetail.Itineraries);

                // Create the offer
                var offer = new FlightDataApiModel
                {
                    Type = offerDetail.Type,
                    Id = offerDetail.Id,
                    Source = offerDetail.Source,
                    InstantTicketingRequired = offerDetail.InstantTicketingRequired,
                    NonHomogeneous = offerDetail.NonHomogeneous,
                    OneWay = offerDetail.OneWay,
                    LastTicketingDate = offerDetail.LastTicketingDate,
                    NumberOfBookableSeats = offerDetail.NumberOfBookableSeats,
                    Itineraries = itineraries,
                    Price = offerDetail.Price,
                    PricingOptions = offerDetail.PricingOptions,
                    ValidatingAirlineCodes = offerDetail.ValidatingAirlineCodes,
                    TravelerPricings = offerDetail.TravelerPricings
                };

                // Persist the offer
                await context.FlightOffers.AddAsync(new FlightOfferDataModel
                {
                    Data = JsonSerializer.Serialize(offer),
                    AmaClientRef = offerDetail.AmaClientRef
                });

                // Save changes
                await context.SaveChangesAsync();

                // Create the offers price model
                var offersPriceRequest = new OffersPriceApiModel
                {
                    Data = new OffersPriceData
                    {
                        FlightOffers = [
                            offer
                        ]
                    }
                };

                // Send the request for price verification
                var response = await amadeusService.VerifyOffersPriceAsync(offersPriceRequest, offerDetail.AmaClientRef);

                // If price verification failed...
                if (!response.Successful)
                {
                    // Log error
                    logger.LogError(response.ErrorMessage);

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = response.ErrorResult.Errors[0].Title,
                        StatusCode = response.ErrorResult.Errors[0].Status,
                        ErrorMessage = response.ErrorResult.Errors[0].Detail
                    };
                }

                // Extract the verified flight offer
                var flightOffer = response.Result.Data.FlightOffers[0];

                // Set the pricing result
                var verificationResult = new FlightOfferDetail
                {
                    Type = offerDetail.Type,
                    Id = offerDetail.Id,
                    Source = offerDetail.Source,
                    AmaClientRef = offerDetail.AmaClientRef,
                    InstantTicketingRequired = offerDetail.InstantTicketingRequired,
                    NonHomogeneous = offerDetail.NonHomogeneous,
                    OneWay = offerDetail.OneWay,
                    LastTicketingDate = offerDetail.LastTicketingDate,
                    NumberOfBookableSeats = offerDetail.NumberOfBookableSeats,
                    DurationInMinutes = offerDetail.DurationInMinutes,
                    Itineraries = offerDetail.Itineraries,
                    Price = flightOffer.Price,
                    PricingOptions = flightOffer.PricingOptions,
                    ValidatingAirlineCodes = flightOffer.ValidatingAirlineCodes,
                    TravelerPricings = flightOffer.TravelerPricings
                };

                // Return result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status200OK,
                    Result = verificationResult
                };
            }
            catch (Exception ex)
            {
                // Log error
                logger.LogError(ex.Message);

                // Return error result
                return new OperationResult
                {
                    ErrorTitle = "SYSTEM ERROR",
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}