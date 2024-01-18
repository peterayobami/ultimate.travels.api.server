namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The payment tranactions domain service
    /// </summary>
    public class PaymentTransaction
    {
        #region Private Members

        /// <summary>
        /// The scoped instance of the <see cref="ApplicationDbContext"/>
        /// </summary>
        private readonly ApplicationDbContext context;

        #endregion

        #region Constructor

        public PaymentTransaction(ApplicationDbContext context)
        {
            this.context = context;
        }

        #endregion
    }
}