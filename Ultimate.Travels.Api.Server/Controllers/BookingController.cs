using Microsoft.AspNetCore.Mvc;

namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// Manages standard Web API requests for flight booking operations
    /// </summary>
    public class BookingController : Controller
    {
        #region Private Members

        /// <summary>
        /// The singleton instance of the <see cref="ILogger{TCategoryName}"/>
        /// </summary>
        private readonly ILogger<BookingController> logger;

        /// <summary>
        /// The scoped instance of the <see cref="BookingOperations"/>
        /// </summary>
        private readonly BookingOperations bookingOperations;

        #endregion

        #region Controller

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        public BookingController(ILogger<BookingController> logger, BookingOperations bookingOperations)
        {
            this.logger = logger;
            this.bookingOperations = bookingOperations;
        }

        #endregion

        /// <summary>
        /// Entry point to initialize flight booking with specified credentials
        /// </summary>
        /// <param name="bookingCredentials">The specified booking credentials</param>
        /// <returns></returns>
        [HttpPost(EndpointRoutes.InitializeFlightBooking)]
        public async Task<ActionResult> InitializeBookingAsync([FromBody] InitializeBookingCredentials bookingCredentials)
        {
            try
            {
                // Trigger the operation
                var operation = await bookingOperations.InitializeAsync(bookingCredentials);

                // If operation failed...
                if (!operation.Successful)
                {
                    // Return error response
                    return Problem(title: operation.ErrorTitle,
                        statusCode: operation.StatusCode, detail: operation.ErrorMessage);
                }

                // Return response
                return Created(string.Empty, operation.Result);
            }
            catch (Exception ex)
            {
                // Log the error
                logger.LogError(ex.Message);

                // Return error response
                return Problem(title: "SYSTEM ERROR",
                    statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
            }
        }

        /// <summary>
        /// Entry point to create booking based on initialized flight booking
        /// </summary>
        /// <param name="bookingCredentials">The specified booking credentials</param>
        /// <returns></returns>
        [HttpPost(EndpointRoutes.CreateFlightBooking)]
        public async Task<ActionResult> CreateBookingAsync([FromBody] CreateBookingCredentials bookingCredentials)
        {
            try
            {
                // Trigger the operation
                var operation = await bookingOperations.CreateAsync(bookingCredentials);

                // If operation failed...
                if (!operation.Successful)
                {
                    // TODO: Communicate full error result

                    // Return error response
                    return Problem(title: operation.ErrorTitle,
                        statusCode: operation.StatusCode, detail: operation.ErrorMessage);
                }

                // Return response
                return Created(string.Empty, operation.Result);
            }
            catch(Exception ex)
            {
                // Log the error
                logger.LogError(ex.Message);

                // Return error response
                return Problem(title: "SYSTEM ERROR", 
                    statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
            }
        }

        /// <summary>
        /// Entry point to retrieve booking
        /// </summary>
        /// <param name="id">The specified booking credentials</param>
        /// <returns></returns>
        [HttpGet(EndpointRoutes.FetchFlightBooking)]
        public async Task<ActionResult> FetchBookingAsync([FromQuery] string id)
        {
            try
            {
                // Trigger the operation
                var operation = await bookingOperations.FetchAsync(id);

                // If operation failed...
                if (!operation.Successful)
                {
                    // Return error response
                    return Problem(title: operation.ErrorTitle,
                        statusCode: operation.StatusCode, detail: operation.ErrorMessage);
                }

                // Return response
                return Ok(operation.Result);
            }
            catch (Exception ex)
            {
                // Log the error
                logger.LogError(ex.Message);

                // Return error response
                return Problem(title: "SYSTEM ERROR",
                    statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
            }
        }
    }
}
