using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AgriEnergyConnect.Models
{
    public class Employee
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        [JsonIgnore]
        public string FirstName { get; set; }

        [Required]
        [JsonIgnore]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [JsonIgnore]
        public string Email { get; set; }
        [JsonIgnore]
        public string Department { get; set; }
        [JsonIgnore]
        public string Position { get; set; }

        [JsonIgnore]  // To prevent serialization of sensitive data
        public string PasswordHash { get; set; }
    }
}