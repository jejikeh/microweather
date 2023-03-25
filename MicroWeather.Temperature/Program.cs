using Microsoft.EntityFrameworkCore;
using MicroWeather.Temperature.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TemperatureDbContext>(
    optionsBuilder =>
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("Temperature"));

    }, ServiceLifetime.Transient
);

var app = builder.Build();
app.Run();