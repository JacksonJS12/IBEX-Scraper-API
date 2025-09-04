# IBEX-Scraper-API

A lightweight **ASP.NET Core Web API** that automatically scrapes **day-ahead electricity prices from IBEX (Independent Bulgarian Energy Exchange)** and stores them in a remote SQL Server database for use by external applications.

**Live API:** [link](http://ibex-scraper.somee.com/swagger/index.html) *(HTTP only – free hosting plan limitation)*
If didn't open check the address for https and change it to http

---

## ⚡ Purpose

This API was built to **decouple IBEX access from dependent systems** by providing a secure, reliable, and filterable data backend.

It powers apps like [SunNext](https://github.com/JacksonJS12/SunNext) by allowing them to:

* Access daily price data without scraping IBEX directly
* Query historical electricity prices by date/hour
* Build custom dashboards and energy trading strategies

---

## 📊 What It Does

* Scrapes **IBEX Day Ahead** prices for the next day
* Extracts hourly energy prices (BGN/MWh)
* Saves results into a `MarketPrices` table with:

  * `Date` (format: `dd/MM/yyyy`)
  * `Hour` (1–24 using EET/EEST timezone)
  * `PricePerMWh` (BGN/MWh)

---

## 🕒 Scheduling

* Prices are updated daily after **14:00 BG time (UTC+3)** when IBEX publishes the next day’s market prices.
* Data scraping is triggered by calling:

  ```
  POST /api/Scrape/scrape-and-save
  ```
* Only stores **new** data (prevents duplicate entries for the same date/hour)
* **Scheduler:** Configured using [cron-job.org](https://cron-job.org) to automatically call the above endpoint every day at **14:00 EEST**.

---

## 🧱 Tech Stack

* **Backend:** ASP.NET Core Web API (.NET 8)
* **Scraper:** AngleSharp
* **Database:** MS SQL (Somee.com hosting)
* **Deployment:** FTP to Somee.com (Free Hosting Plan, HTTP-only)
* **Scheduler:** [cron-job.org](https://cron-job.org) external HTTP request trigger

---

## 🌐 Deployment Notes (Somee Free Plan)

* Hosted on **Somee.com** free plan
* **HTTP only** – no HTTPS due to free hosting limitations
* API runs at: [http://ibex-scraper.somee.com](http://ibex-scraper.somee.com)
* Connection string stored in `appsettings.Production.json` on the server
* Deployment via FTP to `/www.ibex-scraper.somee.com`

---

## 📁 Project Structure

```
IBEX-Scraper-API/
├── Controllers/
│   └── ScrapeController.cs
├── Services/
│   └── IbexScraper/
│       ├── IbexScraperService.cs
│       └── IIbexScraperService.cs
├── Data/
│   ├── Migrations/
│   ├── Models/
│   │   └── MarketPrice.cs
│   ├── AppDbContext.cs
│   └── AppDbContextFactory.cs
└── Program.cs
```

