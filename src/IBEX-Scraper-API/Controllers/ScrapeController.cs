using IBEX_Scraper_API.Data;
using IBEX_Scraper_API.Services.IbexScraper;
using Microsoft.AspNetCore.Mvc;

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
                var today = DateTime.UtcNow.Date;

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

    }
}
