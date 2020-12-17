using System.Text.Json.Serialization;

namespace ApiTest.Models
{
    public class Salary
    {
        [JsonPropertyName("from")]
        public double? From { get; set; }

        [JsonPropertyName("to")]
        public double? To { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("gross")]
        public bool? Gross { get; set; }
    }
}
