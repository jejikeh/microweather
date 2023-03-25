using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MicroWeather.Report.Model;

namespace MicroWeather.Report.Persistence;

public class WeatherReportConfiguration : IEntityTypeConfiguration<WeatherReport>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<WeatherReport> builder)
    {
        builder.HasKey(weatherReport => weatherReport.Id);
        builder.HasIndex(weatherReport => weatherReport.Id).IsUnique();
    }
}