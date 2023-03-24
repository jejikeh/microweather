using Microsoft.EntityFrameworkCore;

namespace MicroWeather.Precipitation.Persistence;

public class PrecipitationDbContext : DbContext
{
    public DbSet<Model.Precipitation> Precipitations { get; set; }

    public PrecipitationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new PrecipitationConfiguration());
    }
}