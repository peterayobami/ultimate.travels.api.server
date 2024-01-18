namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The payment status constants
    /// </summary>
    public class PaymentStatus
    {
        /// <summary>
        /// Describes a failed payment transaction
        /// </summary>
        public const string Failed = "FAILED";

        /// <summary>
        /// Describes a pending payment transaction
        /// </summary>
        public const string Pending = "PENDING";

        /// <summary>
        /// Describes a successful payment transaction
        /// </summary>
        public const string Successful = "SUCCESSFUL";
    }
}