namespace MicroWeather.Temperature.Model;

public class TemperatureViewModel
{
    public decimal TempHighC { get; set; }
    public DateTime CreateOn { get; set; }
    public decimal TempLowC { get; set; }
    public string ZipCode { get; set; } = "undefined";
}