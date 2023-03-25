using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroWeather.Precipitation.Persistence;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<PrecipitationDbContext>(
            optionsBuilder =>
            {
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.EnableDetailedErrors();
                optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("Precipitation"));

            }, ServiceLifetime.Transient
        );

        var app = builder.Build();

        app.MapGet("/observation/{zip}", async (
            string zip,
            [FromQuery] int? days,
            PrecipitationDbContext context) =>
        {
            if (days is null or < 0 or > 30)
                return Results.BadRequest("Provide a valid days value between 1 and 30");

            var startDate = DateTime.Now - TimeSpan.FromDays(-days.Value);
            var result = await context
                .Precipitations
                .Where(p => p.ZipCode == zip && p.CreatedOn < startDate)
                .ToListAsync();

            return Results.Ok(result);
        });

        app.Run();
    }
}