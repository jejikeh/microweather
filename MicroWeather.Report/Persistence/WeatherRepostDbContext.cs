using Microsoft.EntityFrameworkCore;
using MicroWeather.Report.Model;

namespace MicroWeather.Report.Persistence;
public class WeatherReportDbContext : DbContext
{
    public DbSet<WeatherReport> WeatherReports { get; set; }

    public WeatherReportDbContext(DbContextOptions<WeatherReportDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new WeatherReportConfiguration());
    }
}