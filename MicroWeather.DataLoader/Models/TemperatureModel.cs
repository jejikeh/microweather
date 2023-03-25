namespace MicroWeather.DataLoader.Models;

public class TemperatureModel
{
    public decimal TempHighC { get; set; }
    public DateTime CreatedOn { get; set; }
    public decimal TempLowC { get;set; }
    public string ZipCode { get; set; }
}