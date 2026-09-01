using System.Text.Json;
using CurrencyApi.Models;

namespace CurrencyApi.Services
{
    public class AltinkaynakService : ICurrencyService
    {
        private readonly HttpClient _httpClient;

        public AltinkaynakService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CurrencyDto>> GetAllRatesAsync()
        { 
            // Get the rates from the Altinkaynak API
            var jsonString = await _httpClient.GetStringAsync("https://static.altinkaynak.com/public/Currency");

            using var document = JsonDocument.Parse(jsonString);
            
            // RootElement is a list with [ ] brackets
            var root = document.RootElement; 
            var dtoList = new List<CurrencyDto>();

            // Root is already a list, so we can directly enumerate the array
            foreach (var currencyElement in root.EnumerateArray())
            {
                var currencyCode = currencyElement.GetProperty("Kod").GetString();
                var buyingRateStr = currencyElement.GetProperty("Alis").GetString();
                var sellingRateStr = currencyElement.GetProperty("Satis").GetString();

                // Critical touch: The comma in the "48,100" data from Altinkaynak, 
                // to prevent our universal converter (InvariantCulture) from getting confused, we convert it to a dot ("48.100").
                buyingRateStr = buyingRateStr?.Replace(",", ".");
                sellingRateStr = sellingRateStr?.Replace(",", ".");
                // Convert the string to a decimal
                var buyingRate = ParseDecimal(buyingRateStr);
                var sellingRate = ParseDecimal(sellingRateStr);

                // Add the currency to the list
                dtoList.Add(new CurrencyDto
                {
                    Code = currencyCode ?? "Bilinmiyor",
                    BuyingRate = buyingRate,
                    SellingRate = sellingRate
                });
            }

            return dtoList; // Return the list of currencies
        }

        public async Task<CurrencyDto> GetRateAsync(string code)
        {
            var allRates = await GetAllRatesAsync();
            return allRates.FirstOrDefault(x => x.Code.ToUpper() == code.ToUpper());
        }

        // Same method as in TcmbService to safely convert the string to a decimal
        private decimal ParseDecimal(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;
            
            if (decimal.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }
            return 0;
        }
    }
}