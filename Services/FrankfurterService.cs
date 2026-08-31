using System.Net.Http.Json;
using System.Text.Json.Serialization;
using CurrencyApi.Models;

namespace CurrencyApi.Services
{
    public class FrankfurterRate
    {
        [JsonPropertyName("quote")]
        public string Quote { get; set; } = string.Empty;

        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
    }

    public class FrankfurterService : ICurrencyService
    {
        private readonly HttpClient _httpClient;

        public FrankfurterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CurrencyDto>> GetAllRatesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<FrankfurterRate>>(
                "https://api.frankfurter.dev/v2/rates");

            if (response == null || response.Count == 0)
                return new List<CurrencyDto>();

            return response.Select(rate => new CurrencyDto
            {
                Code = rate.Quote,
                BuyingRate = rate.Rate,
                SellingRate = rate.Rate,
            });
        }

        public async Task<CurrencyDto?> GetRateAsync(string code)
        {
            var allRates = await GetAllRatesAsync();
            return allRates.FirstOrDefault(x =>
                string.Equals(x.Code, code, StringComparison.OrdinalIgnoreCase));
        }
    }
}
