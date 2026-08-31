using CurrencyApi.Services;
using Microsoft.AspNetCore.Mvc;
using CurrencyApi.Models;

namespace CurrencyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public ExchangeController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("frankfurter")]
        public async Task<IActionResult> GetFrankfurterRates()
        {
            var rates = await _currencyService.GetAllRatesAsync();
            return Ok(rates);
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetRate(string code)
        {
            var rate = await _currencyService.GetRateAsync(code);
            if (rate == null)
                return NotFound($"{code} kodlu döviz kuru bulunamadı.");

            return Ok(rate);
        }
        // Döviz dönüştürme işlemi
        [HttpGet("convert")]
        public async Task<IActionResult> ConvertCurrency([FromQuery] string from, [FromQuery] string to, [FromQuery] decimal amount)
        {
            // Döviz kurlarını al
            var fromRate = await _currencyService.GetRateAsync(from);
            var toRate = await _currencyService.GetRateAsync(to);
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
    }
}
