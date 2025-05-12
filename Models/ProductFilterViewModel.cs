namespace AgriEnergyConnect.Models
{
    public class ProductFilterViewModel
    {
        [JsonIgnore]
        public int? FarmerId { get; set; }
        [JsonIgnore]
        public string? Category { get; set; }
        [JsonIgnore]
        public DateTime? StartDate { get; set; }
        [JsonIgnore]
        public DateTime? EndDate { get; set; }
        [JsonIgnore]
        public IEnumerable<Product>? Products { get; set; }
        [JsonIgnore]
        public IEnumerable<Farmer>? Farmers { get; set; }  // For dropdown
    }
}