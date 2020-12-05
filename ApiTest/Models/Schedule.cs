using System.Text.Json.Serialization;

namespace ApiTest.Models
{
    public class Schedule
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
