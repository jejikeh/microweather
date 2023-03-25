using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroWeather.Report.Model;

namespace MicroWeather.Report.Core;

public interface IWeatherReportAggregator
{
    public Task<WeatherReport> BuildWeatherReport(string zip, int days);        
}