using System.Text.Json.Serialization;

namespace Ultimate.Travels.Api.Server
{
    /// <summary>
    /// The API model for representing error during fetching authorization error
    /// </summary>
    public class AuthorizationErrorApiModel
    {
        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; }

        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
}