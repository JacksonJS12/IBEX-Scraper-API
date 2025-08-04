# IBEX-Scraper-API

A secure ASP.NET Core Web API that automatically scrapes **day-ahead prices from the Free Electricity Market (Day Ahead segment) on IBEX** and stores them in a structured Azure SQL database for use by external applications.

Access it at [link](https://ibex-scraper-api-e5bwewdyfacgetaf.swedencentral-01.azurewebsites.net/swagger/index.html)
## ⚡ Purpose

This API was built to **decouple IBEX access from dependent systems** by providing a secure, reliable, and filterable data backend.

It powers apps like [SunNext](https://github.com/JacksonJS12/SunNext) by allowing them to:
- Access daily price data without scraping IBEX directly
- Query historical electricity prices by date/hour
- Build custom dashboards and energy trading strategies

## 📊 What It Does

- Scrapes **IBEX Day Ahead segment** prices for the next day
- Extracts hourly energy prices (EUR/MWh)
- Saves results into an Azure SQL `MarketPrices` table with:
  - `Date` (e.g. `day/month/year`)
  - `Hour` (1–24 using Central European Time – CET Time Zone)
  - `PricePerMWh` (BGN/MWh)

## 🕒 Scheduling

- Powered by an **Azure Logic App** that triggers daily at **14:00 BG time (UTC+3)**
- Calls the `POST /api/Scrape/scrape-and-save` endpoint
- Only stores new data (prevents duplicate entries for the same day)
  
  <center><img width="394" height="356" alt="image" src="https://github.com/user-attachments/assets/897ffe35-0acf-4eba-b825-cbbb05ec982d" /></center> 

## 🧱 Tech Stack

- **Backend:** ASP.NET Core Web API (.NET 8)
- **Scraper:** AngleSharp
- **Database:** Azure SQL
- **Scheduler:** Azure Logic Apps
- **Deployment:** Azure App Service
- **CI/CD:** GitHub Actions

📁 Project Structure
IBEX-Scraper-API/ <br/>
├── Controllers/ <br/>
│     └── ScrapeController.cs <br/>
├── Services/ <br/>
│     └── IbexScraper/ <br/>
│         └── IbexScraperService.cs <br/>
│         └── IIbexScraperService.cs <br/>
├── Data/ <br/>
│     └── Migrations/ <br/>
│     └── Models/ <br/>
│         └── MarketPrice.cs <br/>
│     └── AppDbContext.cs <br/>
│     └── AppDbContextFactory.cs <br/>
└── Program.cs <br/>

