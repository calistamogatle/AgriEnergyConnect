namespace AgriEnergyConnect.Models.Account
{
    public class User
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public string Email { get; set; }
        [JsonIgnore]
        public string PasswordHash { get; set; }
        [JsonIgnore]
        public string Role { get; set; }
    }
}
