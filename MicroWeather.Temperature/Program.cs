using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroWeather.Temperature.Model;
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

app.MapGet("/observation/{zip}", async (string zip, [FromQuery] int? days, TemperatureDbContext context) => {
    if (days is null or < 0 or > 30)
        return Results.BadRequest("Provide a valid days value between 1 and 30");

    var startDate = DateTime.Now - TimeSpan.FromDays(-days.Value);
    var result = await context.Temperatures
        .Where(p => p.ZipCode == zip && p.CreatedOn < startDate)
        .ToListAsync();

    return Results.Ok(result);
});

app.MapPost("/observation", async (TemperatureViewModel temperatureViewModel, TemperatureDbContext context) => {
    var temperature = new Temperature(){
        Id = Guid.NewGuid(),
        CreatedOn = temperatureViewModel.CreateOn,
        TempHighC = temperatureViewModel.TempHighC,
        TempLowC = temperatureViewModel.TempLowC,
        ZipCode = temperatureViewModel.ZipCode
    };
    
    await context.AddAsync(temperature);
    await context.SaveChangesAsync();
});

app.Run();