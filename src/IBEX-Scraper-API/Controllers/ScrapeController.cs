using IBEX_Scraper_API.Data;
using IBEX_Scraper_API.Data.Models;
using IBEX_Scraper_API.Services.IbexScraper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IBEX_Scraper_API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ScrapeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IIbexScraper _scraper;

        public ScrapeController(AppDbContext context, IIbexScraper scraper)
        {
            _context = context;
            _scraper = scraper;
        }

        [HttpPost("scrape-and-save")]
        public async Task<IActionResult> ScrapeAndSave()
        {
            try
            {
                // Get current time in EEST
                TimeZoneInfo eestZone = TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time");
                DateTime eestTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, eestZone);

                // Prices come at 14:00 EEST, so if the current time is before that, we will not scrape today’s data. 
                // If it is after 14:00 EEST, we will scrape data for the next day.
                var today = eestTime.Hour < 14 ? eestTime.Date : eestTime.AddDays(1).Date;

                bool alreadyExists = _context.MarketPrices.Any(p => p.Date == today.ToString("dd/MM/yyyy"));
                if (alreadyExists)
                {
                    return Ok(new { Message = "Data already exists for today. Skipping scrape." });
                }

                var data = await _scraper.ScrapeAsync();

                foreach (var item in data)
                {
                    bool exists = _context.MarketPrices.Any(p =>
                        p.Date == item.Date && p.Hour == item.Hour);

                    if (!exists)
                        _context.MarketPrices.Add(item);
                }

                await _context.SaveChangesAsync();

                return Ok(new { Saved = data.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = ex.Message,
                    Stack = ex.StackTrace
                });
            }
        }
        [HttpGet("prices-per-date")]
        public async Task<IActionResult> GetDataFromTheDatabase([FromQuery] DateTime date)
        {
            try
            {
                var marketPrices = await _context.MarketPrices
                    .Where(p => p.Date == date.Date.ToString("dd/MM/yyyy"))
                    .OrderBy(p => p.Hour)
                    .ToListAsync();

                if (marketPrices == null || !marketPrices.Any())
                {
                    return NotFound(new { Message = "There is no data in the database for the specified date." });
                }

                var result = marketPrices.Select(p => new
                {
                    p.Date,
                    p.Hour,
                    p.PricePerMWh
                });

                return Ok(result.OrderBy(x => x.Hour));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = ex.Message,
                    Stack = ex.StackTrace
                });
            }
        }

        [HttpGet("get-all-prices")]
        public async Task<IActionResult> GetAllPrices()
        {
            try
            {
                var marketPrices = await _context.MarketPrices
                    .OrderBy(p => p.Hour)
                    .ToListAsync();

                if (marketPrices == null || !marketPrices.Any())
                {
                    return NotFound(new { Message = "There is no data in the database." });
                }

                var result = marketPrices.Select(p => new
                {
                    p.Date,
                    p.Hour,
                    p.PricePerMWh
                });
                
                return Ok(result.OrderBy(p => p.Date).ThenBy(p => p.Hour));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = ex.Message,
                    Stack = ex.StackTrace
                });
            }
        }
    }
}
