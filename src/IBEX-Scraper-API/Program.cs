using IBEX_Scraper_API.Data;
using IBEX_Scraper_API.Services.IbexScraper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IIbexScraper, IbexScraper>();

var app = builder.Build();

//if (builder.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
