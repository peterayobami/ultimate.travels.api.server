using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// Manages Web API requests for flight shopping
    /// </summary>
    public class ShoppingController : ControllerBase
    {
        #region Private Members

        /// <summary>
        /// The scoped instance of the <see cref="ShoppingOperations"/>
        /// </summary>
        private readonly ShoppingOperations shoppingOperations;

        /// <summary>
        /// The singleton instance of the <see cref="ILogger"/>
        /// </summary>
        private readonly ILogger<ShoppingController> logger;

        #endregion

        #region Constructor

        public ShoppingController(ShoppingOperations shoppingOperations, ILogger<ShoppingController> logger)
        {
            this.shoppingOperations = shoppingOperations;
            this.logger = logger;
        }

        #endregion

        /// <summary>
        /// Handles processing and storage of flight request
        /// </summary>
        /// <param name="requestCredentials">The specified request credentials</param>
        /// <returns></returns>
        [HttpPost(EndpointRoutes.PersistFlightRequest)]
        public async Task<ActionResult> PersistRequestAsync([FromBody] FlightRequestCredentials requestCredentials)
        {
            try
            {
                // Trigger the operation
                var operation = await shoppingOperations.PersistRequestAsync(requestCredentials);

                // If operation failed...
                if (!operation.Successful)
                {
                    // Log error
                    logger.LogError(operation.ErrorMessage);

                    // Return error response
                    return Problem(title: operation.ErrorTitle,
                        statusCode: operation.StatusCode, detail: operation.ErrorMessage);
                }

                // Return response
                return Created(string.Empty, operation.Result);
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex.Message);

                // Return error response
                return Problem(title: "SYSTEM ERROR", detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Fetches flight offers based on a previously persisted search credentials
        /// </summary>
        /// <param name="requestId">The flight request id</param>
        /// <returns></returns>
        [HttpGet(EndpointRoutes.FetchFlightOffers)]
        public async Task<ActionResult> FetchOffersAsync([FromQuery] string requestId)
        {
            try
            {
                // Trigger the operation
                var operation = await shoppingOperations.FetchOffersAsync(requestId);

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
                // Log the exception
                logger.LogError(ex.Message);

                // Return error response
                return Problem(title: "SYSTEM ERROR", detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Fetches flight offers based on provided flight request credentials
        /// </summary>
        /// <param name="requestCredentials">The specified request credentials</param>
        /// <returns></returns>
        [HttpPost(EndpointRoutes.FetchFlightOffers)]
        public async Task<ActionResult> FetchOffersAsync([FromBody] FlightRequestCredentials requestCredentials)
        {
            try
            {
                // Trigger the operation
                var operation = await shoppingOperations.FetchOffersAsync(requestCredentials);

                // If operation failed...
                if (!operation.Successful)
                {
                    // Log error
                    logger.LogError(operation.ErrorMessage);

                    // Return error response
                    return Problem(title: operation.ErrorTitle,
                        statusCode: operation.StatusCode, detail: operation.ErrorMessage);
                }

                // Return response
                return Created(string.Empty, operation.Result);
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex.Message);

                // Return error response
                return Problem(title: "SYSTEM ERROR", detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Initiates price verification
        /// </summary>
        /// <param name="offerDetail">The specified offer detail</param>
        /// <returns></returns>
        [HttpPost(EndpointRoutes.VerifyOffersPricing)]
        public async Task<ActionResult> VerifyOffersPriceAsync([FromBody] FlightOfferDetail offerDetail)
        {
            try
            {
                // Trigger the operation
                var operation = await shoppingOperations.VerifyOffersPriceAsync(offerDetail);

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
                // Log error
                logger.LogError(ex.Message);

                // Return error response
                return Problem(title: "SYSTEM ERROR",
                    statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
            }
        }
    }
}