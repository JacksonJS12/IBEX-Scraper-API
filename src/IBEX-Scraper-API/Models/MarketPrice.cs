namespace IBEX_Scraper_API.Models
{
    public class MarketPrice
    {
        public MarketPrice()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public int Hour { get; set; }
        public decimal PricePerMWh { get; set; }
        public string Date { get; set; }
    }
}
