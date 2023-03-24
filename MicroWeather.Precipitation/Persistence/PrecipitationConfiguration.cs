using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroWeather.Precipitation.Persistence;

public class PrecipitationConfiguration : IEntityTypeConfiguration<Model.Precipitation>
{
    public void Configure(EntityTypeBuilder<Model.Precipitation> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => p.Id).IsUnique();
    }
}