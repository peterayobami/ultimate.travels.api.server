namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The REST API response model for Amadeus HTTP transactions
    /// </summary>
    public class AmadeusApiResult
    {
        /// <summary>
        /// True, if error message is null
        /// </summary>
        public bool Successful => ErrorMessage == null;

        /// <summary>
        /// The error message, if any
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The result data of the HTTP transaction
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// Warning, if any
        /// </summary>
        public object WarningResult { get; set; }

        /// <summary>
        /// Errors, if any
        /// </summary>
        public object ErrorResult { get; set; }
    }

    public class AmadeusApiResult<TResult, TWarningResult, TErrorResult> : AmadeusApiResult
    {
        public new TResult Result { get; set; }

        public new TWarningResult WarningResult { get; set; }

        public new TErrorResult ErrorResult { get; set; }
    }
}