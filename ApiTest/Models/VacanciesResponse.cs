using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiTest.Models
{
    public class VacanciesResponse
    {
        [JsonPropertyName("items")]
        public List<Vacancy> Vacancies { get; set; }

        [JsonPropertyName("found")]
        public int Count { get; set; }

        [JsonPropertyName("pages")]
        public int Pages { get; set; }

        [JsonPropertyName("per_page")]
        public int PerPage { get; set; }
    }
}
