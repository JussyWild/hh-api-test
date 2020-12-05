using System;
using System.Text.Json.Serialization;

namespace ApiTest.Models
{
    public class Vacancy
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("premium")]
        public bool Premium { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("department")]
        public object Department { get; set; }

        [JsonPropertyName("has_test")]
        public bool HasTest { get; set; }

        [JsonPropertyName("response_letter_required")]
        public bool ResponseLetterRequired { get; set; }

        [JsonPropertyName("area")]
        public Area Area { get; set; }

        [JsonPropertyName("salary")]
        public Salary Salary { get; set; }

        [JsonPropertyName("type")]
        public Type Type { get; set; }

        [JsonPropertyName("address")]
        public Address Address { get; set; }

        [JsonPropertyName("employer")]
        public Employer Employer { get; set; }

        [JsonPropertyName("archived")]
        public bool Archived { get; set; }

        [JsonPropertyName("apply_alternate_url")]
        public string ApplyAlternateUrl { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("alternate_url")]
        public string AlternateUrl { get; set; }

        [JsonPropertyName("snippet")]
        public Snippet Snippet { get; set; }

        [JsonPropertyName("schedule")]
        public Schedule Schedule { get; set; }

        [JsonPropertyName("accept_temporary")]
        public bool AcceptTemporary { get; set; }
    }
}
