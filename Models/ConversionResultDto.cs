namespace CurrencyApi.Models
{
    public class ConversionResultDto
    {
        public string? FromCurrency { get; set; }
        public string? ToCurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
        public decimal ConvertedAmount { get; set; }
    }
}