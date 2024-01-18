using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// Service that handles all that relates to Amadeus authorization
    /// </summary>
    public class AmadeusAuthorizationService
    {
        #region Private Members

        /// <summary>
        /// The scoped instance of the <see cref="IHttpClientFactory"/>
        /// </summary>
        private readonly IHttpClientFactory httpClient;

        /// <summary>
        /// The singleton instance of the <see cref="ILogger"/>
        /// </summary>
        private readonly ILogger<AmadeusAuthorizationService> logger;

        /// <summary>
        /// The singleton instance of the <see cref="AmadeusConfiguration"/>
        /// </summary>
        private readonly AmadeusConfiguration amadeusConfiguration;

        /// <summary>
        /// The scoped instance of the <see cref="ApplicationDbContext"/>
        /// </summary>
        private readonly ApplicationDbContext context;

        #endregion

        #region Constructor

        public AmadeusAuthorizationService(IHttpClientFactory httpClient, ILogger<AmadeusAuthorizationService> logger,
            AmadeusConfiguration amadeusConfiguration, ApplicationDbContext context)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.amadeusConfiguration = amadeusConfiguration;
            this.context = context;
        }

        #endregion

        /// <summary>
        /// Fetches access token from Amadeus
        /// </summary>
        /// <returns></returns>
        public async Task<AmadeusApiResult<AuthorizationCredentialsApiModel, object, AuthorizationErrorApiModel>> GetAccessTokenAsync()
        {
            try
            {
                // Create the client
                using var client = httpClient.CreateClient();

                // Create the payload of the request
                var clientCredentials = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", amadeusConfiguration.ClientId),
                    new KeyValuePair<string, string>("client_secret", amadeusConfiguration.ClientSecret)
                });

                // Trigger the request
                var response = await client.PostAsync($"{amadeusConfiguration.Host}{AmadeusEndpoints.GetAccessToken}", clientCredentials);

                // Initialize response body
                string responseBody = default;

                // If request failed...
                if (!response.IsSuccessStatusCode)
                {
                    // Extract the response body
                    responseBody = await response.Content.ReadAsStringAsync();

                    // Deserialize the response
                    var errorResult = JsonSerializer.Deserialize<AuthorizationErrorApiModel>(responseBody);

                    // Set the error message
                    string errorMessage = $"Token Request Failed: Title: {errorResult.Title}, Error: {errorResult.Error}, Description: {errorResult.ErrorDescription}, Code: {errorResult.Code}";

                    // Log the error
                    logger.LogError(errorMessage);

                    // Return error result
                    return new AmadeusApiResult<AuthorizationCredentialsApiModel, object, AuthorizationErrorApiModel>
                    {
                        ErrorMessage = errorMessage,
                        ErrorResult = errorResult
                    };
                }

                // Extract the result body
                responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the result
                var result = JsonSerializer.Deserialize<AuthorizationCredentialsApiModel>(responseBody);

                // Return result
                return new AmadeusApiResult<AuthorizationCredentialsApiModel, object, AuthorizationErrorApiModel>
                {
                    Result = result
                };
            }
            catch (Exception ex)
            {
                // Log exception
                logger.LogError(ex.Message);

                // Return error result
                return new AmadeusApiResult<AuthorizationCredentialsApiModel, object, AuthorizationErrorApiModel>
                {
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Updates the Amadeus Access Token with new requested token
        /// </summary>
        /// <returns></returns>
        public async Task UpdateAccessTokenAsync()
        {
            try
            {
                // Get the token
                var tokenRequest = await GetAccessTokenAsync();

                // If request failed...
                if (!tokenRequest.Successful)
                {
                    // Log the error
                    logger.LogError("Access Token cannot be updated due to failure in token request");

                    // Exit method
                    return;
                }

                // Get the result
                var result = tokenRequest.Result;

                // Retrieve existing Amadeus credentials
                var existingCredentials = await context.AmadeusCredentials.FirstOrDefaultAsync();

                // If we don't have existing credentials...
                if (existingCredentials == null)
                {
                    // Set the authorization credentials
                    var amadeusCredentials = new AmadeusCredentialsDataModel
                    {
                        Type = result.Type,
                        Username = result.Username,
                        ClientId = result.ClientId,
                        TokenType = result.TokenType,
                        AccessToken = result.AccessToken,
                        State = result.State,
                        Scope = result.Scope
                    };

                    // Create the credentials
                    await context.AmadeusCredentials.AddAsync(amadeusCredentials);
                }
                // Otherwise...
                else
                {
                    // Modify the existing credentials
                    existingCredentials.Type = result.Type;
                    existingCredentials.Username = result.Username;
                    existingCredentials.ClientId = result.ClientId;
                    existingCredentials.TokenType = result.TokenType;
                    existingCredentials.AccessToken = result.AccessToken;
                    existingCredentials.State = result.State;
                    existingCredentials.Scope = result.Scope;

                    // Update the credentials
                    context.AmadeusCredentials.Update(existingCredentials);
                }

                // Save changes
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                logger.LogError(ex.Message);
            }
        }
    }
}