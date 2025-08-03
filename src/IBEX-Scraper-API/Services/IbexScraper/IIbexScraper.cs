using IBEX_Scraper_API.Models;

namespace IBEX_Scraper_API.Services.IbexScraper
{
    public interface IIbexScraper
    {
        Task<List<MarketPrice>> ScrapeAsync();
    }
}
