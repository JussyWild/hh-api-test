using System.Text.Json.Serialization;

namespace ApiTest.Models
{
    public class Snippet
    {
        [JsonPropertyName("requirement")]
        public string Requirement { get; set; }

        [JsonPropertyName("responsibility")]
        public string Responsibility { get; set; }
    }
}
