using CurrencyApi.Services;
using Microsoft.AspNetCore.Mvc;
using CurrencyApi.Models;

namespace CurrencyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeController : ControllerBase
    {
        private readonly ICurrencyService _frankfurterService;
        private readonly ICurrencyService _tcmbService;
        private readonly ICurrencyService _altinkaynakService;

        public ExchangeController(
            [FromKeyedServices("Frankfurter")] ICurrencyService frankfurterService,
            [FromKeyedServices("Tcmb")] ICurrencyService tcmbService,
            [FromKeyedServices("Altinkaynak")] ICurrencyService altinkaynakService)
        {
            _frankfurterService = frankfurterService;
            _tcmbService = tcmbService; 
            _altinkaynakService = altinkaynakService;
        }

        [HttpGet("frankfurter")]
        public async Task<IActionResult> GetFrankfurterRates()
        {
            var rates = await _frankfurterService.GetAllRatesAsync();
            return Ok(rates);
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetRate(string code)
        {
            var rate = await _frankfurterService.GetRateAsync(code);
            if (rate == null)
                return NotFound($"{code} kodlu döviz kuru bulunamadı.");

            return Ok(rate);
        }
        // Döviz dönüştürme işlemi
        [HttpGet("convert")]
        public async Task<IActionResult> ConvertCurrency([FromQuery] string from, [FromQuery] string to, [FromQuery] decimal amount)
        {
            // Döviz kurlarını al
            var fromRate = await _frankfurterService.GetRateAsync(from);
            var toRate = await _frankfurterService.GetRateAsync(to);
            // Döviz kurlarını alamadıysa hata döndür
            if (fromRate == null || toRate == null)
                return NotFound("Belirtilen para birimlerinden biri veya ikisi bulunamadı.");
            // Çapraz kuru hesapla
            var crossRate = toRate.SellingRate / fromRate.SellingRate;
            var calculatedAmount = amount * crossRate;

            // Dönüştürme sonucunu döndür
            var result = new ConversionResultDto
            {
                FromCurrency = from.ToUpper(),
                ToCurrency = to.ToUpper(),
                Amount = amount,
                Rate = Math.Round(crossRate, 4), // 4 basamağa yuvarlıyoruz 
                ConvertedAmount = Math.Round(calculatedAmount, 2) // Parayı 2 basamağa (kuruş) yuvarlıyoruz
            };

            return Ok(result);
        }

        [HttpGet("tcmb")]
        public async Task<IActionResult> GetTcmbRates()
        {
            // Get the rates from the TCMB service
            var rates = await _tcmbService.GetAllRatesAsync();
            // Return the rates
            return Ok(rates);
        }

        [HttpGet("altinkaynak")]
        public async Task<IActionResult> GetAltinkaynakRates()
        {
            // Get the rates from the Altinkaynak service
            var rates = await _altinkaynakService.GetAllRatesAsync();
            // Return the rates
            return Ok(rates);
        }
    }
}
