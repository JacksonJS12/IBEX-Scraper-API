namespace IBEX_Scraper_API.Data.Models
{
    public class MarketPrice
    {
        public MarketPrice()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public int Hour { get; set; }
        public decimal PricePerMWh { get; set; }
        public string Date { get; set; }
    }
}
