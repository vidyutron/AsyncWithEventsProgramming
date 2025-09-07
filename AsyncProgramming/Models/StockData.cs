using System.Text.Json.Serialization;

namespace AsyncProgramming.Models
{
    public class StockData
    {
        [JsonPropertyName("symbol")]
        public required string Symbol { get; set; }

        [JsonPropertyName("company_name")]
        public required string CompanyName { get; set; }

        [JsonPropertyName("sector")]
        public string? Sector { get; set; }

        [JsonPropertyName("currency")]
        public required string Currency { get; set; }

        [JsonPropertyName("last_updated")]
        public DateTime LastUpdated { get; set; }

        [JsonPropertyName("daily_data")]
        public required List<DailyPrice> DailyData { get; set; }
    }
}
