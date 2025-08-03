using IBEX_Scraper_API.Data.Models;

namespace IBEX_Scraper_API.Services.IbexScraper
{
    public interface IIbexScraper
    {
        Task<List<MarketPrice>> ScrapeAsync();
    }
}
