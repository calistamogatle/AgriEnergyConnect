using System.Diagnostics;

namespace AgriEnergyConnect.Models
{
    public class ErrorViewModel
    {
        [JsonIgnore]
        /// <summary>
        /// The unique request identifier from Activity.Current or HttpContext
        /// </summary>
        public string RequestId { get; set; } = Activity.Current?.Id ?? Guid.NewGuid().ToString();

        [JsonIgnore]
        /// <summary>
        /// Determines whether to display the RequestId in the UI
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        [JsonIgnore]
        /// <summary>
        /// The HTTP status code (e.g., 404, 500)
        /// </summary>
        public int StatusCode { get; set; }
        [JsonIgnore]
        /// <summary>
        /// The original request path where the error occurred
        /// </summary>
        public string RequestPath { get; set; }
        [JsonIgnore]
        /// <summary>
        /// Timestamp when the error occurred (UTC)
        /// </summary>
        public DateTime ErrorTimeUtc { get; } = DateTime.UtcNow;
    }
}