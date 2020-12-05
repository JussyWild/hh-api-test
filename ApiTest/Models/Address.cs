using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiTest.Models
{
    public class Address
    {
        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("building")]
        public string Building { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("lat")]
        public double? Lat { get; set; }

        [JsonPropertyName("lng")]
        public double? Lng { get; set; }

        [JsonPropertyName("raw")]
        public string Raw { get; set; }

        [JsonPropertyName("metro")]
        public Metro Metro { get; set; }

        [JsonPropertyName("metro_stations")]
        public List<MetroStation> MetroStations { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
