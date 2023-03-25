using Microsoft.EntityFrameworkCore;

namespace MicroWeather.Temperature.Persistence;
public class TemperatureDbContext : DbContext
{
    public DbSet<Model.Temperature> Temperatures { get; set; }

    public TemperatureDbContext(DbContextOptions<TemperatureDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new TemperatureConfiguration());
    }
}