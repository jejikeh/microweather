namespace MicroWeather.DataLoader.Models;

public class TemperatureModel
{
    public decimal TemperatureHighC { get; set; }
    public DateTime CreatedOn { get; set; }
    public decimal TemperatureLowC { get;set; }
    public string ZipCode { get; set; }
}