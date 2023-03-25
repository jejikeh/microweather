namespace MicroWeather.Report.Model;
public class WeatherReport
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public decimal AverageTemperatureHighC { get; set; }
    public decimal AverageTemperatureLowC { get; set; }
    public decimal RainfallTotalInches { get; set;}
    public decimal SnowTotalInches { get; set; }
    public string ZipCode { get; set; } = "undefined";
}