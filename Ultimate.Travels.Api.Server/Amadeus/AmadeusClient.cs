using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;

namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The Amadeus client that handles HTTP requests
    /// </summary>
    public class AmadeusClient
    {
        #region Private Members

        /// <summary>
        /// The scoped instance of the <see cref="IHttpClientFactory"/>
        /// </summary>
        private readonly IHttpClientFactory httpClient;

        /// <summary>
        /// The scoped instance of the <see cref="ApplicationDbContext"/>
        /// </summary>
        private readonly ApplicationDbContext context;

        /// <summary>
        /// The singleton instance of the <see cref="ILogger"/>
        /// </summary>
        private readonly ILogger<AmadeusClient> logger;

        /// <summary>
        /// The singleton instance of the <see cref="AmadeusConfiguration"/>
        /// </summary>
        private readonly AmadeusConfiguration amadeusConfiguration;

        #endregion

        #region Constructor

        public AmadeusClient(IHttpClientFactory httpClient, ApplicationDbContext context,
            ILogger<AmadeusClient> logger, AmadeusConfiguration amadeusConfiguration)
        {
            this.httpClient = httpClient;
            this.context = context;
            this.logger = logger;
            this.amadeusConfiguration = amadeusConfiguration;
        }

        #endregion

        /// <summary>
        /// Handles request to Amadeus based on specified parameters and types
        /// </summary>
        /// <typeparam name="TData">The type parameter of the request body</typeparam>
        /// <typeparam name="TResult">The type parameter of the result</typeparam>
        /// <typeparam name="TWarningResult">The type parameter of the warning result</typeparam>
        /// <typeparam name="TErrorResult">The type parameter of the error result</typeparam>
        /// <param name="amadeusRequest">The Amadeus request parameters</param>
        /// <returns>Return <see cref="AmadeusApiResult"/> based on response from Amadeus</returns>
        public async Task<AmadeusApiResult<TResult, TWarningResult, TErrorResult>> SendAsync<TData, TResult, TWarningResult, TErrorResult>(AmadeusRequestApiModel<TData> amadeusRequest)
        {
            try
            {
                // Get the access token
                string accessToken = await context.AmadeusCredentials.Select(a => a.AccessToken).FirstOrDefaultAsync();

                // Create the HTTP client
                using var client = httpClient.CreateClient();

                // Create the request
                JsonSerializerOptions jsonOptions = new()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                // Set the request credentials
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{amadeusConfiguration.Host}{amadeusRequest.Endpoint}"),
                    Method = amadeusRequest.Method,
                    Content = new StringContent(JsonSerializer.Serialize(amadeusRequest.Data, jsonOptions), Encoding.UTF8, "application/json")
                };

                // For each specified header...
                foreach (var header in amadeusRequest.Headers)
                {
                    // Add the header
                    request.Headers.Add(header.Key, header.Value);
                }

                // Add the authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Trigger the request
                var response = await client.SendAsync(request);

                // Initialize response body
                string responseBody = default;

                // If request failed...
                if (!response.IsSuccessStatusCode)
                {
                    // Extract the body of the response
                    responseBody = await response.Content.ReadAsStringAsync();

                    // Deserialize the error result
                    var errorResult = JsonSerializer.Deserialize<TErrorResult>(responseBody);

                    // Log the error
                    logger.LogError($"Failed to Process Request. Detail: {responseBody}");

                    // Return error result
                    return new AmadeusApiResult<TResult, TWarningResult, TErrorResult>
                    {
                        ErrorMessage = "Failed to Process Request",
                        ErrorResult = errorResult
                    };
                }

                // Extract the body of the response
                responseBody = await response.Content.ReadAsStringAsync();
                
                // Deserialize the result
                var result = JsonSerializer.Deserialize<TResult>(responseBody, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                // Return result
                return new AmadeusApiResult<TResult, TWarningResult, TErrorResult>
                {
                    Result = result
                };
            }
            catch (Exception ex)
            {
                // Log the error
                logger.LogError(ex.Message);

                // Return error result
                return new AmadeusApiResult<TResult, TWarningResult, TErrorResult>
                {
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}