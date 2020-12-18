using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using ApiTest.Models;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace ApiTest
{
    public class ApiTest
    {
        private readonly HttpClient _client;
        private readonly Uri _baseUrl;

        public ApiTest()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            _baseUrl = new Uri(configuration.GetSection("BaseUrl").Value);
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("HH-User-Agent", "");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                configuration.GetSection("Token").Value
            );
        }

        [Fact]
        public async Task NegativeTestGet_WithoutUserAgentAndAuthorizationToken_ShouldReturnBadRequest()
        {
            _client.DefaultRequestHeaders.Clear();
            var resp = await _client.GetAsync(_baseUrl);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        [Fact]
        public async Task NegativeTestGet_WithHHUserAgentNoAuthorizationToken_ShouldReturnBadRequest()
        {
            _client.DefaultRequestHeaders.Authorization = null;
            var resp = await _client.GetAsync(_baseUrl);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        [Fact]
        public async Task NegativeTestGet_WithUserAgentNoAuthorizationToken_ShouldReturnBadRequest()
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/87.0.4280.88 Safari/537.36"
            );

            var resp = await _client.GetAsync(_baseUrl);

            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }

        [Fact]
        public async Task PositiveTestGet_WithHHUserAgentAndAuthorizationToken_ShouldReturnOk()
        {
            var resp = await _client.GetAsync(_baseUrl);
            var content = await resp.Content.ReadAsStringAsync();

            Assert.True(resp.IsSuccessStatusCode);
            Assert.False(string.IsNullOrEmpty(content));
            Assert.False(string.IsNullOrWhiteSpace(content));
        }

        [Theory]
        [InlineData("text=Программист")]
        public async Task PositiveTestGet_WithRussianTextQueryParameter_ShouldReturnVacanciesWithWord(string text)
        {
            var builder = new UriBuilder(_baseUrl) { Query = text };
            var resp = await _client.GetAsync(builder.Uri);
            var content = await resp.Content.ReadAsStringAsync();

            Assert.True(resp.IsSuccessStatusCode);
            Assert.False(string.IsNullOrEmpty(content));
            Assert.False(string.IsNullOrWhiteSpace(content));

            var vacanciesResponse = JsonSerializer.Deserialize<VacanciesResponse>(content);
            var isValid = vacanciesResponse.Vacancies.Any(v => IsVacancyContain(v, "Программист"));

            Assert.NotNull(vacanciesResponse);
            Assert.NotNull(vacanciesResponse.Vacancies);
            Assert.NotEmpty(vacanciesResponse.Vacancies);
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("text=Developer")]
        public async Task PositiveTestGet_WithEnglishTextQueryParameter_ShouldReturnVacanciesWithWord(string text)
        {
            var builder = new UriBuilder(_baseUrl) { Query = text };
            var resp = await _client.GetAsync(builder.Uri);
            var content = await resp.Content.ReadAsStringAsync();

            Assert.True(resp.IsSuccessStatusCode);
            Assert.False(string.IsNullOrEmpty(content));
            Assert.False(string.IsNullOrWhiteSpace(content));

            var vacanciesResponse = JsonSerializer.Deserialize<VacanciesResponse>(content);
            var isValid = vacanciesResponse.Vacancies.Any(v => IsVacancyContain(v, "Developer"));

            Assert.NotNull(vacanciesResponse);
            Assert.NotNull(vacanciesResponse.Vacancies);
            Assert.NotEmpty(vacanciesResponse.Vacancies);
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("text=Продажа оборудования")]
        public async Task PositiveTestGet_WithWordForm_ShouldReturnVacanciesWithAnotherWordForm(string text)
        {
            var builder = new UriBuilder(_baseUrl) { Query = text };
            var resp = await _client.GetAsync(builder.Uri);
            var content = await resp.Content.ReadAsStringAsync();

            Assert.True(resp.IsSuccessStatusCode);
            Assert.False(string.IsNullOrEmpty(content));
            Assert.False(string.IsNullOrWhiteSpace(content));

            var vacanciesResponse = JsonSerializer.Deserialize<VacanciesResponse>(content);
            var isValid = vacanciesResponse.Vacancies.Any(v => IsVacancyContain(v, "Продажам оборудования"));

            Assert.NotNull(vacanciesResponse);
            Assert.NotNull(vacanciesResponse.Vacancies);
            Assert.NotEmpty(vacanciesResponse.Vacancies);
            Assert.True(isValid);
        }

        [Theory]
        [InlineData(@"text=""Продажа оборудования""")]
        public async Task PositiveTestGet_WithPhrase_ShouldReturnVacanciesWithPhrase(string text)
        {
            var builder = new UriBuilder(_baseUrl) { Query = text };
            var resp = await _client.GetAsync(builder.Uri);
            var content = await resp.Content.ReadAsStringAsync();

            Assert.True(resp.IsSuccessStatusCode);
            Assert.False(string.IsNullOrEmpty(content));
            Assert.False(string.IsNullOrWhiteSpace(content));

            var vacanciesResponse = JsonSerializer.Deserialize<VacanciesResponse>(content);
            var isValid =
                vacanciesResponse.Vacancies.Any(v => IsVacancyContain(v, "Продажам торгового оборудования"));

            Assert.NotNull(vacanciesResponse);
            Assert.NotNull(vacanciesResponse.Vacancies);
            Assert.NotEmpty(vacanciesResponse.Vacancies);
            Assert.False(isValid);
        }

        [Theory]
        [InlineData("text=!Продажи")]
        public async Task PositiveTestGet_WithStrongWordForm_ShouldReturnVacanciesWithSpecificWord(string text)
        {
            var builder = new UriBuilder(_baseUrl) { Query = text };
            var resp = await _client.GetAsync(builder.Uri);
            var content = await resp.Content.ReadAsStringAsync();

            Assert.True(resp.IsSuccessStatusCode);
            Assert.False(string.IsNullOrEmpty(content));
            Assert.False(string.IsNullOrWhiteSpace(content));

            var vacanciesResponse = JsonSerializer.Deserialize<VacanciesResponse>(content);
            var isValid = vacanciesResponse.Vacancies.All(v => IsVacancyContain(v,"Продажи"));

            Assert.NotNull(vacanciesResponse);
            Assert.NotNull(vacanciesResponse.Vacancies);
            Assert.NotEmpty(vacanciesResponse.Vacancies);
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("text=Гео*")]
        public async Task PositiveTestGet_WithPartOfWord_ShouldReturnVacanciesWithPartOfWord(string text)
        {
            var builder = new UriBuilder(_baseUrl) { Query = text };
            var resp = await _client.GetAsync(builder.Uri);
            var content = await resp.Content.ReadAsStringAsync();

            Assert.True(resp.IsSuccessStatusCode);
            Assert.False(string.IsNullOrEmpty(content));
            Assert.False(string.IsNullOrWhiteSpace(content));

            var vacanciesResponse = JsonSerializer.Deserialize<VacanciesResponse>(content);
            var isValid = vacanciesResponse.Vacancies.All(v => IsVacancyContain(v,"Гео"));

            Assert.NotNull(vacanciesResponse);
            Assert.NotNull(vacanciesResponse.Vacancies);
            Assert.NotEmpty(vacanciesResponse.Vacancies);
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("text=пиарщик")]
        public async Task PositiveTestGet_WithSynonym_ShouldReturnVacanciesSynonyms(string text)
        {
            var builder = new UriBuilder(_baseUrl) { Query = text };
            var resp = await _client.GetAsync(builder.Uri);
            var content = await resp.Content.ReadAsStringAsync();

            Assert.True(resp.IsSuccessStatusCode);
            Assert.False(string.IsNullOrEmpty(content));
            Assert.False(string.IsNullOrWhiteSpace(content));

            var vacanciesResponse = JsonSerializer.Deserialize<VacanciesResponse>(content);
            var isValid = vacanciesResponse.Vacancies.Any(v => IsVacancyContain(v,"PR-менеджер"));

            Assert.NotNull(vacanciesResponse);
            Assert.NotNull(vacanciesResponse.Vacancies);
            Assert.NotEmpty(vacanciesResponse.Vacancies);
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("text=c# or c++")]
        public async Task PositiveTestGet_WithOneOfWord_ShouldReturnVacanciesWithWords(string text)
        {
            var builder = new UriBuilder(_baseUrl) { Query = text };
            var resp = await _client.GetAsync(builder.Uri);
            var content = await resp.Content.ReadAsStringAsync();

            Assert.True(resp.IsSuccessStatusCode);
            Assert.False(string.IsNullOrEmpty(content));
            Assert.False(string.IsNullOrWhiteSpace(content));

            var vacanciesResponse = JsonSerializer.Deserialize<VacanciesResponse>(content);
            Assert.NotNull(vacanciesResponse);
            Assert.NotNull(vacanciesResponse.Vacancies);
            Assert.NotEmpty(vacanciesResponse.Vacancies);

            var isValid = vacanciesResponse.Vacancies.Any(v => IsVacancyContain(v,"c#"));

            if (false == isValid)
            {
                isValid = vacanciesResponse.Vacancies.Any(v => IsVacancyContain(v,"c++"));
            }

            Assert.True(isValid);
        }

        [Theory]
        [InlineData(@"text=""разработчик java"" or ""разработчик c#""")]
        public async Task PositiveTestGet_WithOneOfPhrase_ShouldReturnVacanciesWithPhrases(string text)
        {
            var builder = new UriBuilder(_baseUrl) { Query = text };
            var resp = await _client.GetAsync(builder.Uri);
            var content = await resp.Content.ReadAsStringAsync();

            Assert.True(resp.IsSuccessStatusCode);
            Assert.False(string.IsNullOrEmpty(content));
            Assert.False(string.IsNullOrWhiteSpace(content));

            var vacanciesResponse = JsonSerializer.Deserialize<VacanciesResponse>(content);
            Assert.NotNull(vacanciesResponse);
            Assert.NotNull(vacanciesResponse.Vacancies);
            Assert.NotEmpty(vacanciesResponse.Vacancies);

            var isValid = vacanciesResponse.Vacancies.Any(v => IsVacancyContain(v,"разработчик java"));

            if (false == isValid)
            {
                isValid = vacanciesResponse.Vacancies.Any(v => IsVacancyContain(v,"разработчик c#"));
            }

            Assert.True(isValid);
        }

        [Theory]
        [InlineData(@"text=""python"" and ""django""")]
        public async Task PositiveTestGet_WithAllPhrases_ShouldReturnVacanciesWithAllPhrases(string text)
        {
            var builder = new UriBuilder(_baseUrl) { Query = text };
            var resp = await _client.GetAsync(builder.Uri);
            var content = await resp.Content.ReadAsStringAsync();

            Assert.True(resp.IsSuccessStatusCode);
            Assert.False(string.IsNullOrEmpty(content));
            Assert.False(string.IsNullOrWhiteSpace(content));

            var vacanciesResponse = JsonSerializer.Deserialize<VacanciesResponse>(content);
            Assert.NotNull(vacanciesResponse);
            Assert.NotNull(vacanciesResponse.Vacancies);
            Assert.NotEmpty(vacanciesResponse.Vacancies);

            var isValid = vacanciesResponse.Vacancies.All(v => IsVacancyContain(v,"python"));
            Assert.True(isValid);

            isValid = vacanciesResponse.Vacancies.All(v => IsVacancyContain(v,"django"));
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("text=NAME:(python or java) and COMPANY_NAME:Headhunter")]
        public async Task PositiveTestGet_WithProperties_ShouldReturnVacanciesWithSpecificProperties(string text)
        {
            var builder = new UriBuilder(_baseUrl) { Query = text };
            var resp = await _client.GetAsync(builder.Uri);
            var content = await resp.Content.ReadAsStringAsync();

            Assert.True(resp.IsSuccessStatusCode);
            Assert.False(string.IsNullOrEmpty(content));
            Assert.False(string.IsNullOrWhiteSpace(content));

            var vacanciesResponse = JsonSerializer.Deserialize<VacanciesResponse>(content);
            Assert.NotNull(vacanciesResponse);
            Assert.NotNull(vacanciesResponse.Vacancies);
            Assert.NotEmpty(vacanciesResponse.Vacancies);

            var isValid = vacanciesResponse.Vacancies.Any(v => IsVacancyContain(v,"java", true));

            if (false == isValid)
            {
                isValid = vacanciesResponse.Vacancies.Any(v => IsVacancyContain(v,"python", true));
            }
            Assert.True(isValid);

            isValid = vacanciesResponse.Vacancies.Any(v => v.Employer.Name.ToLower() != "headhunter");
            Assert.False(isValid);
        }

        [Theory]
        [InlineData("text=<script>alert('Executing JS')</script>")]
        public async Task NegativeTestGet_WithXSS_ShouldReturnEmptyResult(string text)
        {
            var builder = new UriBuilder(_baseUrl) { Query = text };
            var resp = await _client.GetAsync(builder.Uri);
            var content = await resp.Content.ReadAsStringAsync();

            Assert.True(resp.IsSuccessStatusCode);
            Assert.False(string.IsNullOrEmpty(content));
            Assert.False(string.IsNullOrWhiteSpace(content));

            var vacanciesResponse = JsonSerializer.Deserialize<VacanciesResponse>(content);
            Assert.NotNull(vacanciesResponse);
            Assert.NotNull(vacanciesResponse.Vacancies);
            Assert.Empty(vacanciesResponse.Vacancies);
        }

        [Theory]
        [InlineData("text=;DROP TABLE Employers;")]
        public async Task NegativeTestGet_WithSqlInjection_ShouldReturnEmptyResult(string text)
        {
            var builder = new UriBuilder(_baseUrl) { Query = text };
            var resp = await _client.GetAsync(builder.Uri);
            var content = await resp.Content.ReadAsStringAsync();

            Assert.True(resp.IsSuccessStatusCode);
            Assert.False(string.IsNullOrEmpty(content));
            Assert.False(string.IsNullOrWhiteSpace(content));

            var vacanciesResponse = JsonSerializer.Deserialize<VacanciesResponse>(content);
            Assert.NotNull(vacanciesResponse);
            Assert.NotNull(vacanciesResponse.Vacancies);
            Assert.Empty(vacanciesResponse.Vacancies);
        }

        [Theory]
        [InlineData("text=fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")]
        public async Task NegativeTestGet_WithWrongWord_ShouldReturnEmptyResult(string text)
        {
            var builder = new UriBuilder(_baseUrl) { Query = text };
            var resp = await _client.GetAsync(builder.Uri);
            var content = await resp.Content.ReadAsStringAsync();

            Assert.True(resp.IsSuccessStatusCode);
            Assert.False(string.IsNullOrEmpty(content));
            Assert.False(string.IsNullOrWhiteSpace(content));

            var vacanciesResponse = JsonSerializer.Deserialize<VacanciesResponse>(content);
            Assert.NotNull(vacanciesResponse);
            Assert.NotNull(vacanciesResponse.Vacancies);
            Assert.Empty(vacanciesResponse.Vacancies);
        }

        [Theory]
        [InlineData("text=Ө`ЎЈ?>")]
        public async Task NegativeTestGet_WithWrongEncoding_ShouldReturnEmptyResult(string text)
        {
            var builder = new UriBuilder(_baseUrl) { Query = text };
            var resp = await _client.GetAsync(builder.Uri);
            var content = await resp.Content.ReadAsStringAsync();

            Assert.True(resp.IsSuccessStatusCode);
            Assert.False(string.IsNullOrEmpty(content));
            Assert.False(string.IsNullOrWhiteSpace(content));

            var vacanciesResponse = JsonSerializer.Deserialize<VacanciesResponse>(content);
            Assert.NotNull(vacanciesResponse);
            Assert.NotNull(vacanciesResponse.Vacancies);
            Assert.Empty(vacanciesResponse.Vacancies);
        }

        [Theory]
        [InlineData("text=       ")]
        public async Task NegativeTestGet_WithOnlySpaces_ShouldReturnVacancies(string text)
        {
            var builder = new UriBuilder(_baseUrl) { Query = text };
            var resp = await _client.GetAsync(builder.Uri);
            var content = await resp.Content.ReadAsStringAsync();

            Assert.True(resp.IsSuccessStatusCode);
            Assert.False(string.IsNullOrEmpty(content));
            Assert.False(string.IsNullOrWhiteSpace(content));

            var vacanciesResponse = JsonSerializer.Deserialize<VacanciesResponse>(content);
            Assert.NotNull(vacanciesResponse);
            Assert.NotNull(vacanciesResponse.Vacancies);
            Assert.NotEmpty(vacanciesResponse.Vacancies);
        }

        [Theory]
        [InlineData("text=")]
        public async Task NegativeTestGet_WithEmptyQueryParameter_ShouldReturnVacancies(string text)
        {
            var builder = new UriBuilder(_baseUrl) { Query = text };
            var resp = await _client.GetAsync(builder.Uri);
            var content = await resp.Content.ReadAsStringAsync();

            Assert.True(resp.IsSuccessStatusCode);
            Assert.False(string.IsNullOrEmpty(content));
            Assert.False(string.IsNullOrWhiteSpace(content));

            var vacanciesResponse = JsonSerializer.Deserialize<VacanciesResponse>(content);
            Assert.NotNull(vacanciesResponse);
            Assert.NotNull(vacanciesResponse.Vacancies);
            Assert.NotEmpty(vacanciesResponse.Vacancies);
        }

        private bool IsVacancyContain(Vacancy vacancy, string value, bool checkOnlyNameProperty = false)
        {
            value = value.Trim().ToLower();
            if (vacancy.Name.ToLower().Contains(value))
            {
                return true;
            }

            if (checkOnlyNameProperty)
            {
                return false;
            }

            if (null != vacancy.Snippet)
            {
                if (null != vacancy.Snippet.Requirement && vacancy.Snippet.Requirement.ToLower().Contains(value))
                {
                    return true;
                }

                if (null != vacancy.Snippet.Responsibility  && vacancy.Snippet.Responsibility.ToLower().Contains(value))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
