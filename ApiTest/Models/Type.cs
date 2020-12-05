using System.Text.Json.Serialization;

namespace ApiTest.Models
{
    public class Type
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
