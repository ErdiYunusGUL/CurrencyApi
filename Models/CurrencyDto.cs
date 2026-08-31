namespace CurrencyApi.Models
{
    public class CurrencyDto
    {
        public string Code { get; set; } = string.Empty;
        public decimal BuyingRate { get; set; }
        public decimal SellingRate { get; set; }
    }
}
