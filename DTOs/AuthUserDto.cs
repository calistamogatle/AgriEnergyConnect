using System.Text.Json.Serialization;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace AgriEnergyConnect.DTO
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
public class AuthUserDto
{
    [JsonPropertyName("id")]
    [JsonIgnore]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    [JsonIgnore]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("fullName")]
    [JsonIgnore]
    public string FullName { get; set; } = string.Empty;

    [JsonPropertyName("role")]
    [JsonIgnore]
    public string Role { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;
}

}
