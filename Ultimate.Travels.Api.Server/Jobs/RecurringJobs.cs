using Hangfire;

namespace Ultimate.Travels.Api.Server
{
    public class RecurringJobs
    {
        #region Private Members

        /// <summary>
        /// The scoped instance of the <see cref="AmadeusAuthorizationService"/>
        /// </summary>
        private readonly AmadeusAuthorizationService authorizationService;

        /// <summary>
        /// The scoped instance of the <see cref="IRecurringJobManager"/>
        /// </summary>
        private readonly IRecurringJobManager recurringJobManager;

        /// <summary>
        /// The scoped instance of the <see cref="BackgroundServices"/>
        /// </summary>
        private readonly BackgroundServices backgroundServices;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="authorizationService">The injected authorization service</param>
        /// <param name="recurringJobManager">The injected recurring job manager</param>
        public RecurringJobs(AmadeusAuthorizationService authorizationService, 
            IRecurringJobManager recurringJobManager, BackgroundServices backgroundServices)
        {
            this.authorizationService = authorizationService;
            this.recurringJobManager = recurringJobManager;
            this.backgroundServices = backgroundServices;
        }

        #endregion

        /// <summary>
        /// Registers recurring job for Amadeus authorization
        /// </summary>
        public void RegisterAmadeusAuthorization()
        {
            recurringJobManager.AddOrUpdate(JobIdentity.UpdateAuthorizationCredentials,
                methodCall: () => authorizationService.UpdateAccessTokenAsync(), CronExpression.UpdateAuthorizationCredentials);
        }

        /// <summary>
        /// Registers the recurring job for Flight Offers Manager
        /// </summary>
        public void RegisterFlightOffersManager()
        {
            recurringJobManager.AddOrUpdate(JobIdentity.FlightOffersManager,
                methodCall: () => backgroundServices.RemoveFlightOffersAsync(), CronExpression.FlightOffersManager);
        }

        /// <summary>
        /// Registers the recurring job for flight request manager
        /// </summary>
        public void RegisterFlightRequestsManager()
        {
            recurringJobManager.AddOrUpdate(JobIdentity.FlightRequestsManager,
                methodCall: () => backgroundServices.RemovePastDueFlightOffersAsync(), CronExpression.FlightRequestsManager);
        }
    }
}