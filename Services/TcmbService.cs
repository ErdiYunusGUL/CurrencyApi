using System.Globalization;
using System.Xml.Linq;
using CurrencyApi.Models;

namespace CurrencyApi.Services
{     //TCMB Service
    public class TcmbService : ICurrencyService
    {
        private readonly HttpClient _httpClient; // HttpClient for TCMB API
         // Constructor for TcmbService
        public TcmbService(HttpClient httpClient)
        {
            _httpClient = httpClient; // Initialize HttpClient
        }

        public async Task<IEnumerable<CurrencyDto>> GetAllRatesAsync()
        {    // Get all rates from TCMB API
            var xmlContent = await _httpClient.GetStringAsync("https://www.tcmb.gov.tr/kurlar/today.xml");
              // Parse XML content
            var doc = XDocument.Parse(xmlContent);
             // Get all currency elements
            var dtoList = doc.Descendants("Currency")
            .Select(element => new CurrencyDto
                {
                    // Get the code of the currency
                    Code = element.Attribute("Kod")?.Value ?? "Bilinmiyor",
                    // Get the buying rate of the currency
                    BuyingRate = ParseDecimal(element.Element("ForexBuying")?.Value),
                    // Get the selling rate of the currency
                    SellingRate = ParseDecimal(element.Element("ForexSelling")?.Value)
                })
                .ToList(); // Convert to List

                return dtoList; // Return the list of currencies
        }

        public async Task<CurrencyDto> GetRateAsync(string code)
        {
            // Get the rate of the currency
            var allRates = await GetAllRatesAsync(); // Get all rates
            return allRates.FirstOrDefault(x => x.Code.ToUpper() == code.ToUpper()); // Return the rate of the currency
        }
          // Parse the decimal value
        private decimal ParseDecimal(string? value)
        {
            // If the value is null or empty, return 0, prevent system crash (Safe coding)
            if (string.IsNullOrWhiteSpace(value)) return 0;
            
            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }
            return 0; // Return 0 if the value is not a decimal
        }
    } 
}