using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AgriEnergyConnect.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email Address")]
        [JsonIgnore]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [JsonIgnore]
        public required string Password { get; set; }

        [Display(Name = "Remember me?")]
        [JsonIgnore]
        public bool RememberMe { get; set; }
    }
}