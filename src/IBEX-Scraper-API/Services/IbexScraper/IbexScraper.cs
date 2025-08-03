using AngleSharp;
using IBEX_Scraper_API.Models;

namespace IBEX_Scraper_API.Services.IbexScraper
{
    public class IbexScraper : IIbexScraper
    {
        public async Task<List<MarketPrice>> ScrapeAsync()
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync("https://ibex.bg/dam-data-chart-2/");

            var priceList = document.QuerySelectorAll("td.column-price")
                .Select(x => decimal.Parse(x.TextContent.Trim())).ToList();

            var hourList = document.QuerySelectorAll("td.column-time_part")
                .Select(x => TimeSpan.Parse(x.TextContent.Trim()).Hours).ToList();

            var dateList = document.QuerySelectorAll("td.column-date_part")
                .Select(x => DateTime.Parse(x.TextContent.Trim()).Date).ToList();

            var prices = new List<MarketPrice>();
            for (int i = 0; i < priceList.Count && i < hourList.Count && i < dateList.Count; i++)
            {
                prices.Add(new MarketPrice
                {
                    PricePerMWh = priceList[i],
                    Hour = hourList[i] + 1, // because we start in IBEX from 1am
                    Date = dateList[i].ToString("dd/MM/yyyy")
                });
            }

            return prices;
        }
    }
}
