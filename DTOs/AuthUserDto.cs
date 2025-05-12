using System.Text.Json.Serialization;

namespace AgriEnergyConnect.DTO

}
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
}

}
