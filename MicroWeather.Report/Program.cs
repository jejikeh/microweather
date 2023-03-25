using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroWeather.Report.Config;
using MicroWeather.Report.Core;
using MicroWeather.Report.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddTransient<IWeatherReportAggregator, WeatherReportAggregator>();
builder.Services.AddOptions();
builder.Services.Configure<WeatherDataConfig>(builder.Configuration.GetSection("WeatherDataConfig"));

builder.Services.AddDbContext<WeatherReportDbContext>(
    optionsBuilder =>
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("WeatherReport"));

    }, ServiceLifetime.Transient
);

var app = builder.Build();

app.MapGet("/weather-report/{zip}", async (string zip, [FromQuery] int? days, IWeatherReportAggregator weatherReportAggregator) =>
{
    if (days is null or > 30 or < 1)
        return Results.BadRequest("Please provide a `days` paramets between 1 and 30");
    
    var weatherReport = await weatherReportAggregator.BuildWeatherReport(zip, days.Value);
    return Results.Ok(weatherReport);
});

app.Run();