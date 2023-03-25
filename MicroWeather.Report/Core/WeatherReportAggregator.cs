using Microsoft.Extensions.Options;
using MicroWeather.Report.Config;
using MicroWeather.Report.Model;
using MicroWeather.Report.Persistence;

namespace MicroWeather.Report.Core;

public class WeatherReportAggregator : IWeatherReportAggregator
{
    private readonly IHttpClientFactory _httpClientFacroty;
    private readonly ILogger<WeatherReportAggregator> _logger;
    private readonly WeatherDataConfig _weatherDataConfig;
    private readonly WeatherReportDbContext _context;

    public WeatherReportAggregator(
        IHttpClientFactory httpClientFacroty, 
        ILogger<WeatherReportAggregator> logger, 
        IOptions<WeatherDataConfig> weatherDataConfig,
        WeatherReportDbContext context)
    {
        _httpClientFacroty = httpClientFacroty;
        _logger = logger;
        _weatherDataConfig = weatherDataConfig.Value;
        _context = context;
    }

    public async Task<WeatherReport> BuildWeatherReport(string zip, int days)
    {
        var client = _httpClientFacroty.CreateClient();
        var precipitationData = await FetchPrecipitationData(client, zip, days);
        var totalSnow = GetTotalWeatherTypePrecipitation("snow", precipitationData);
        var totalRain = GetTotalWeatherTypePrecipitation("rain", precipitationData);
        _logger.LogInformation(
            $"zip: {zip} over last {days} days" +
            $"total snow: {totalSnow}, total rain: {totalRain}");

        var temperatureData = await FetchTemperatureData(client, zip, days);
        var averageHighTemp = Math.Round(temperatureData.Average(t => t.TemperatureHighC), 1);
        var averageLowTemp = Math.Round(temperatureData.Average(t => t.TemperatureLowC), 1);
        _logger.LogInformation(
           $"zip: {zip} over last {days} days" +
           $"average high temp: {averageHighTemp}, average low temp: {averageLowTemp}");

        var weatherReport = new WeatherReport() 
        { 
            Id = Guid.NewGuid(), 
            AverageTemperatureHighC = averageHighTemp, 
            AverageTemperatureLowC =  averageLowTemp, 
            RainfallTotalInches = totalRain, 
            SnowTotalInches = totalSnow, 
            ZipCode = zip,
            CreatedOn = DateTime.UtcNow
        };

        await _context.AddAsync(weatherReport);
        await _context.SaveChangesAsync();

        return weatherReport;
    }

    private static decimal GetTotalWeatherTypePrecipitation(string weatherType, IEnumerable<PrecipitationModel> precipitationModels)
    {
        var total = precipitationModels.Where(p => p.WeatherType == weatherType).Sum(p => p.AmmountInches);
        return Math.Round(total, 1);
    }

    private async Task<ICollection<TemperatureModel>> FetchTemperatureData(HttpClient client, string zip, int days)
    {
        var endpoint = BuildSeviceEndPoint(_weatherDataConfig.TemperatureConfig, zip, days);
        var temperature = await client.GetAsync(endpoint);
        var temperatureData = await temperature.Content.ReadFromJsonAsync<List<TemperatureModel>>();
        return temperatureData ?? new List<TemperatureModel>();
    }

    private async Task<ICollection<PrecipitationModel>> FetchPrecipitationData(HttpClient client, string zip, int days)
    {
        var endpoint = BuildSeviceEndPoint(_weatherDataConfig.PrecipitationConfig, zip, days);
        var precipitationJson = await client.GetAsync(endpoint);
        var precipitationData = await precipitationJson.Content.ReadFromJsonAsync<List<PrecipitationModel>>();
        return precipitationData ?? new List<PrecipitationModel>();
    }

    private static string BuildSeviceEndPoint(ServiceConfig config, string zip, int days)
    {
        var protocol = config.Protocol;
        var host = config.Host;
        var port = config.Port;
        return $"{protocol}://{host}:{port}/observation/{zip}?days={days}";
    }
}