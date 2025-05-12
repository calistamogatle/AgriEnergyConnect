using System.Text.Json.Serialization;
using AgriEnergyConnect.DTO;
using AgriEnergyConnect.Models.Account;
using Newtonsoft.Json;

namespace AgriEnergyConnect.Services
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterViewModel model);
        Task<AuthResult> LoginAsync(LoginViewModel model);
        Task<AuthUser?> GetUserByIdAsync(string userId);
    }

    public class AuthResult
    {
        [JsonProperty("success")]
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonProperty("token")]
        [JsonPropertyName("token")]
        public string? Token { get; set; }
        [JsonProperty("user")]
        [JsonPropertyName("user")]
        public AuthUserDto? User { get; set; }
        [JsonProperty("errors")]
        [JsonPropertyName("errors")]
        public IEnumerable<string>? Errors { get; set; }
    }
}