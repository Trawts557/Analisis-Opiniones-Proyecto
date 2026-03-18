
using AnalisisOpiniones.Data.Entities.Api;
using AnalisisOpiniones.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace AnalisisOpiniones.Data.Persistence.Repositories.Api
{
    public class ApiReaderRepository : IApiReaderRepository<ApiModel>
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ApiReaderRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<ApiModel>> ReadAsync(CancellationToken cancellationToken = default)
        {
            var endpoint = _configuration["ExtractionSettings:ApiUrl"];

            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new InvalidOperationException("No se encontró la URL de la API en appsettings.json.");
            }

            using var response = await _httpClient.GetAsync(endpoint, cancellationToken);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken);

            var result = JsonSerializer.Deserialize<List<ApiModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new List<ApiModel>();
        }
    }
}

