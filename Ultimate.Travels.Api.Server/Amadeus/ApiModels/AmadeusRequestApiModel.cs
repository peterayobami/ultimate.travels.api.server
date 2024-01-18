namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The HTTP request properties for any Amadeus request
    /// </summary>
    public class AmadeusRequestApiModel<TData>
    {
        /// <summary>
        /// The API endpoint for this request
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// The HTTP request method
        /// </summary>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// The headers for this request
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// The body of the request
        /// </summary>
        public TData Data { get; set; }
    }
}