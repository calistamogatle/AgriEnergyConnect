using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AgriEnergyConnect.Models.Account
{
    public class AuthUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        [JsonIgnore]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password hash is required")]
        [JsonIgnore]
        public string PasswordHash { get; set; } = null!;

        [Required(ErrorMessage = "Role is required")]
        [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters")]
        [JsonIgnore]
        public string Role { get; set; } = null!;
        [JsonIgnore]
        public string? RefreshToken { get; set; }
        [JsonIgnore]
        public DateTime? RefreshTokenExpiry { get; set; }

        // Additional fields as needed
        [StringLength(100)]
        [JsonIgnore]
        public string? FullName { get; set; }

        [StringLength(20)]
        [JsonIgnore]
        public string? PhoneNumber { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public DateTime? LastLogin { get; set; }

        public static implicit operator AuthUser(AuthUserDto v)
        {
            throw new NotImplementedException();
        }
    }

}