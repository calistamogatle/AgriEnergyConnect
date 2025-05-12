using System.Text.Json.Serialization;
using AgriEnergyConnect.DTO;
using Newtonsoft.Json;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace AgriEnergyConnect.Models.Account
{
    public class AuthResponse
    {
        [JsonPropertyName("success")]
        [JsonProperty("success")]
        [JsonIgnore]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        [JsonProperty("message")]
        [JsonIgnore]
        public required string Message { get; set; }

        [JsonPropertyName("token")]
        [JsonProperty("token")]
        [JsonIgnore]
        public required string Token { get; set; }

        [JsonPropertyName("refreshToken")]
        [JsonProperty("refreshToken")]
        [JsonIgnore]
        public required string RefreshToken { get; set; }

        [JsonPropertyName("redirectUrl")]
        [JsonProperty("redirectUrl")]
        [JsonIgnore]
        public required string RedirectUrl { get; set; }

        [JsonPropertyName("user")]
        [JsonProperty("user")]
        [JsonIgnore]
        public required AuthUserDto User { get; set; }

        [JsonPropertyName("errorDetails")]
        [JsonProperty("errorDetails")]
        [JsonIgnore]
        public required string ErrorDetails { get; set; }
    }
}