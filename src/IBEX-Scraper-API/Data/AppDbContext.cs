using IBEX_Scraper_API.Models;
using Microsoft.EntityFrameworkCore;

namespace IBEX_Scraper_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<MarketPrice> MarketPrices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MarketPrice>()
                .HasKey(mp => mp.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
