using System.Text.Json.Serialization;

namespace ApiTest.Models
{
    public class Employer
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("alternate_url")]
        public string AlternateUrl { get; set; }

        [JsonPropertyName("vacancies_url")]
        public string VacanciesUrl { get; set; }

        [JsonPropertyName("trusted")]
        public bool Trusted { get; set; }
    }
}
