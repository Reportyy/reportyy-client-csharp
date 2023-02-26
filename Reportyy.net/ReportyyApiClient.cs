using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Reportyy
{
    public class ReportyyApiClient : IReportyyApiClient
    {
        private const string BASE_URL = "https://api.reportyy.com/";
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _httpClient = new HttpClient();

        public ReportyyApiClient(string apiKey)
            : this(apiKey, BASE_URL)
        {
        }

        public ReportyyApiClient(string apiKey, string baseUrl)
        {
            _httpClient.BaseAddress = new Uri(baseUrl);
            _httpClient.Timeout = new TimeSpan(0, 0, 30);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"API-Key {apiKey}");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = false
            };
        }

        public async Task<Stream> GeneratePDF(string templateId, object data)
        {
            var json = JsonSerializer.Serialize(data, _options);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");
            var url = $"api/v1/templates/{templateId}/generate-sync";

            var response = await _httpClient.PostAsync(url, payload);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorResponseStream = await response.Content.ReadAsStreamAsync();

                try
                {
                    var error = await JsonSerializer.DeserializeAsync<ReportyyApiError>(errorResponseStream);

                    throw new ReportyyApiException($"Reportyy HTTP API Error: {response.StatusCode}", error);
                }
                catch (JsonException)
                {
                    throw new ReportyyApiException($"Reportyy HTTP API Error: {response.StatusCode}");
                }
            }

            return await response.Content.ReadAsStreamAsync();
        }
    }
}

