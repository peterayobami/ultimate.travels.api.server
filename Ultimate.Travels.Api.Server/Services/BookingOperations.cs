using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The booking operations domain service
    /// </summary>
    public class BookingOperations
    {
        #region Private Members

        /// <summary>
        /// The scoped instance of the <see cref="ApplicationDbContext"/>
        /// </summary>
        private readonly ApplicationDbContext context;

        /// <summary>
        /// The scoped instance of the <see cref="ILogger{TCategoryName}"/>
        /// </summary>
        private readonly ILogger<BookingOperations> logger;

        /// <summary>
        /// The scoped instance of the <see cref="AmadeusService"/>
        /// </summary>
        private readonly AmadeusService amadeusService;

        #endregion

        #region Constructor

        public BookingOperations(ApplicationDbContext context, 
            ILogger<BookingOperations> logger, AmadeusService amadeusService)
        {
            this.context = context;
            this.logger = logger;
            this.amadeusService = amadeusService;
        }

        #endregion

        /// <summary>
        /// Creates the booking credentials for further processing
        /// </summary>
        /// <param name="bookingCredentials">The soecified booking credentials</param>
        /// <returns></returns>
        public async Task<OperationResult> InitializeAsync(InitializeBookingCredentials bookingCredentials)
        {
            try
            {
                // Retrieve the flight offer
                var offerRecord = await context.FlightOffers.FirstOrDefaultAsync(o => o.AmaClientRef ==  bookingCredentials.AmaClientRef);

                // If offer was not found...
                if (offerRecord == null)
                {
                    // Log error
                    logger.LogError("The ama client ref is invalid or flight offer has expired");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "BAD REQUEST",
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "The ama client ref is invalid or flight offer has expired"
                    };
                }

                // Deserialize the offer
                var offer = JsonSerializer.Deserialize<FlightDataApiModel>(offerRecord.Data);

                // Fetch the customer
                var customer = await context.Customers.FirstOrDefaultAsync(c => c.Email == bookingCredentials.TravelerContact.Email);

                // If customer was not found...
                if (customer == null)
                {
                    // Set the customer credentials
                    customer = new CustomersDataModel
                    {
                        Email = bookingCredentials.TravelerContact.Email,
                        Phone = bookingCredentials.TravelerContact.Phone,
                        CountryDialingCode = bookingCredentials.TravelerContact.CountryDialingCode,
                        FirstName = bookingCredentials.TravelerContact.FirstName,
                        LastName = bookingCredentials.TravelerContact.LastName
                    };

                    // Create the customer
                    await context.Customers.AddAsync(customer);
                }

                // Set the flight booking credentials
                var flightBooking = new FlightBookingDataModel
                {
                    CustomerId = customer.Id,
                    AmaClientRef = bookingCredentials.AmaClientRef,
                    BookingStatus = BookingStatus.Initialized,
                    PaymentStatus = PaymentStatus.Pending
                };

                // Create the flight booking
                await context.FlightBookings.AddAsync(flightBooking);

                // Initialize travelers
                List<TravelerDataModel> travelers = [];

                // For each traveler...
                foreach (var traveler in bookingCredentials.TravelerCredentials)
                {
                    // Get the correcponding traveler pricing
                    var travelerPricing = offer.TravelerPricings.FirstOrDefault(t => t.TravelerId == traveler.Id);

                    // If traveler pricing is not found...
                    if (travelerPricing == null)
                    {
                        // Log error
                        logger.LogError($"The traveler id: {traveler.Id} is invalid for traveler: {traveler.FirstName} {traveler.LastName}");

                        // Return error result
                        return new OperationResult
                        {
                            ErrorTitle = "BAD REQUEST",
                            StatusCode = StatusCodes.Status400BadRequest,
                            ErrorMessage = $"The traveler id: {traveler.Id} is invalid for traveler: {traveler.FirstName} {traveler.LastName}"
                        };
                    }

                    // If traveler type is not valid...
                    if (traveler.Type != travelerPricing.TravelerType)
                    {
                        // Log error
                        logger.LogError($"Invalid traveler type for {traveler.FirstName} {traveler.LastName}");

                        // Return error result
                        return new OperationResult
                        {
                            ErrorTitle = "BAD REQUEST",
                            StatusCode = StatusCodes.Status400BadRequest,
                            ErrorMessage = $"Invalid traveler type for {traveler.FirstName} {traveler.LastName}. Ensure that the travelers' sequential order correspond to traveler pricings in the selected flight offer"
                        };
                    }

                    // If date of birth is not valid...
                    if (!traveler.DateOfBirth.IsValid(traveler.Type))
                    {
                        // Log the error
                        logger.LogError($"The specified date of birth for traveler: {traveler.FirstName} {traveler.LastName} is not valid.");

                        // Return error result
                        return new OperationResult
                        {
                            ErrorTitle = "BAD REQUEST",
                            StatusCode = StatusCodes.Status400BadRequest,
                            ErrorMessage = $"The specified date of birth for traveler: {traveler.FirstName} {traveler.LastName} is not valid."
                        };
                    }

                    // Add traveler to the collection
                    travelers.Add(new TravelerDataModel
                    {
                        FlightBookingId = flightBooking.Id,
                        Title = traveler.Title,
                        FirstName = traveler.FirstName,
                        LastName = traveler.LastName,
                        Gender = traveler.Gender,
                        TravelerId = traveler.Id,
                        TravelerType = traveler.Type,
                        DateOfBirth = traveler.DateOfBirth.ToString("yyyy-MM-dd")
                    });
                }

                // Create the travelers
                await context.Travelers.AddRangeAsync(travelers);

                // Save changes
                await context.SaveChangesAsync();

                // Return result
                return new OperationResult
                {
                    Result = new { flightBookingId = flightBooking.Id },
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (Exception ex)
            {
                // Log the exception
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
    
        /// <summary>
        /// Completes the booking process of specified initialized booking
        /// </summary>
        /// <param name="bookingCredentials">The specified booking credentials</param>
        /// <returns></returns>
        public async Task<OperationResult> CreateAsync(CreateBookingCredentials bookingCredentials)
        {
            try
            {
                // Fetch the booking
                var flightBooking = await context.FlightBookings
                    .Include(b => b.Customer)
                    .Include(b => b.Travelers)
                    .FirstOrDefaultAsync(b => b.Id == bookingCredentials.FlightBookingId);

                // If booking could not be found...
                if (flightBooking == null)
                {
                    // Log error
                    logger.LogError("Booking not found");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "NOT FOUND",
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Booking not found"
                    };
                }

                // If booking has been processed...
                if (flightBooking.BookingStatus != BookingStatus.Initialized)
                {
                    // Log error
                    logger.LogError("Specified booking has been processed");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "BAD REQUEST",
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "Specified booking has been processed"
                    };
                }

                // Retrieve the flight offer
                var offerRecord = await context.FlightOffers.FirstOrDefaultAsync(o => o.AmaClientRef == flightBooking.AmaClientRef);

                // If offer was not found...
                if (offerRecord == null)
                {
                    // Log error
                    logger.LogError("The ama client ref is invalid or flight offer has expired");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "BAD REQUEST",
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "The ama client ref is invalid or flight offer has expired"
                    };
                }

                // Deserialize the offer
                var offer = JsonSerializer.Deserialize<FlightDataApiModel>(offerRecord.Data);

                // Compose the flight booking credentials
                var credentials = BookingProcessUtility.ComposeCredentialsAsync(offer, flightBooking);

                // Trigger the booking request
                var response = await amadeusService.BookFlightAsync(credentials, flightBooking.AmaClientRef);

                // If booking failed...
                if (!response.Successful)
                {
                    // Log error
                    logger.LogError(response.ErrorMessage);

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "SYSTEM ERROR",
                        StatusCode = StatusCodes.Status500InternalServerError,
                        ErrorMessage = response.ErrorMessage,
                        ErrorResult = response.ErrorResult
                    };
                }

                // Update the booking status
                flightBooking.BookingStatus = BookingStatus.Confirmed;
                // Update order id
                flightBooking.OrderId = response.Result.Data.Id;
                // Update PNR
                flightBooking.Pnr = response.Result.Data.AssociatedRecords[0].Reference;

                // Update flight booking
                context.FlightBookings.Update(flightBooking);

                // Save changes
                await context.SaveChangesAsync();

                // Return result
                return new OperationResult
                {
                    Result = flightBooking,
                    StatusCode = StatusCodes.Status201Created
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

        /// <summary>
        /// Retrieves an existing booking with specified id
        /// </summary>
        /// <param name="id">The specified booking id</param>
        /// <returns></returns>
        public async Task<OperationResult> FetchAsync(string id)
        {
            try
            {
                // Fetch the booking
                var flightBooking = await context.FlightBookings
                    .Include(b => b.Customer)
                    .Include(b => b.Travelers)
                    .FirstOrDefaultAsync(b => b.Id == id);

                // If booking could not be found...
                if (flightBooking == null)
                {
                    // Log error
                    logger.LogError("Booking not found");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "NOT FOUND",
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Booking not found"
                    };
                }

                // If booking has not been processed...
                if (flightBooking.BookingStatus == BookingStatus.Initialized)
                {
                    // Log error
                    logger.LogError("Specified booking is yet to be completed");

                    // Return error result
                    return new OperationResult
                    {
                        ErrorTitle = "BAD REQUEST",
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "Specified booking is yet to be completed"
                    };
                }

                // TODO: Retrieve booking from Amadeus

                // Return result
                return new OperationResult
                {
                    Result = flightBooking,
                    StatusCode = StatusCodes.Status201Created
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