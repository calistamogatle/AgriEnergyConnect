using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AgriEnergyConnect.Models
{
    public class Farmer
    {
        [Key] // Explicitly mark as primary key  
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment  
        [JsonIgnore]
        public int FarmerID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be 2-100 characters")]
        [JsonIgnore]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        [JsonIgnore]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(100)]
        [JsonIgnore]
        public required string Location { get; set; }

        [Required]
        [JsonIgnore] // Never include in API responses  
        public required string PasswordHash { get; set; }
        [JsonIgnore]

        // Navigation property  
        public virtual ICollection<Product> Products { get; set; } = [];

        [Required] // Mark as required to ensure non-nullable  
        [StringLength(100)]
        [JsonIgnore]
        public required string UserEmail { get; set; }
    }
}