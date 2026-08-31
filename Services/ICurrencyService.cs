using CurrencyApi.Models;

namespace CurrencyApi.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyDto>> GetAllRatesAsync();
        Task<CurrencyDto?> GetRateAsync(string code);
    }
}
