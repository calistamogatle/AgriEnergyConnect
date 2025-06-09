using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AgriEnergyConnect.Models
{
    public class Product
    {
        [Key]
        [System.Text.Json.Serialization.JsonIgnore]
        public int ProductID { get; set; }

        [Required]
        [StringLength(100)]
        [JsonPropertyName("name")]
        [JsonIgnore]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [JsonIgnore]
        public required string Category { get; set; }

        [DataType(DataType.Date)]
        [JsonIgnore]
        public DateTime ProductionDate { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        [JsonIgnore]
        public string? Description { get; set; }

        [ForeignKey("Farmer")]
        [JsonIgnore]
        public int FarmerID { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual required Farmer Farmer { get; set; }

        [JsonProperty("category")]
        [JsonPropertyName("category")]

        public string CategoryName
        {
            get => Category;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Category cannot be null or empty");
                }

                Category = value;
            }
        }
    } }