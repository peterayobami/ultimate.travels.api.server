using Microsoft.EntityFrameworkCore;

namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The service for flight offers management
    /// </summary>
    public class BackgroundServices
    {
        #region Private Members

        /// <summary>
        /// The scoped instance of the <see cref="ApplicationDbContext"/>
        /// </summary>
        public readonly ApplicationDbContext context;

        /// <summary>
        /// The singleton instance of the <see cref="ApplicationDbContext"/>
        /// </summary>
        private readonly ILogger<BackgroundServices> logger;

        #endregion

        #region Constructor

        public BackgroundServices(ApplicationDbContext context, ILogger<BackgroundServices> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        #endregion

        /// <summary>
        /// Removes flight offers within the last hour from the database
        /// </summary>
        /// <returns></returns>
        public async Task RemoveFlightOffersAsync()
        {
            try
            {
                // Retrieve offer within the last hour
                var offers = await context.FlightOffers
                    .Where(o => o.DateCreated < DateTime.UtcNow.AddHours(-1)).ToListAsync();

                // Delete retrieved offers
                context.FlightOffers.RemoveRange(offers);

                // Save changes
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log error
                logger.LogError($"Failed to delete flight offers due to following error: {ex.Message}");
            }
        }

        /// <summary>
        /// Removes past due flight requests from database
        /// </summary>
        /// <returns></returns>
        public async Task RemovePastDueFlightOffersAsync()
        {
            try
            {
                // Retrieve all past due flight requests
                var pastDueRequests = await context.FlightRequests
                    .Where(r => r.DepartureDate < DateTime.UtcNow).ToListAsync();

                // Delete the retrieved flight requests
                context.FlightRequests.RemoveRange(pastDueRequests);

                // Save changes
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                logger.LogError($"Failed to delete past due flight requests due to an error: {ex.Message}");
            }
        }
    }
}
