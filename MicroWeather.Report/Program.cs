using Microsoft.EntityFrameworkCore;
using MicroWeather.Report.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WeatherReportDbContext>(
    optionsBuilder =>
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("WeatherReport"));

    }, ServiceLifetime.Transient
);

var app = builder.Build();

app.Run();