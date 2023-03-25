using Microsoft.EntityFrameworkCore;

namespace MicroWeather.Temperature.Persistence;

class TemperatureConfiguration : IEntityTypeConfiguration<Model.Temperature>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Model.Temperature> builder)
    {
        builder.HasKey(temp => temp.Id);
        builder.HasIndex(temp => temp.Id).IsUnique();
    }
}