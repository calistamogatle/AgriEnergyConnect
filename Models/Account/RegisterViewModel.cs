using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace AgriEnergyConnect.Models.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email Address")]  // Fixed: Changed from DisplayName to Display
        [JsonProperty("email")]
        [JsonIgnore]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]  // Fixed: Changed from DisplayName to Display
        [JsonPropertyName("password")]
        [JsonIgnore]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]  // Fixed: Changed from DisplayName to Display
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [JsonIgnore]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}