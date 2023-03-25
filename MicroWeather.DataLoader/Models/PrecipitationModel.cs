namespace MicroWeather.DataLoader.Models;

public class PrecipitationModel
{
    public decimal AmmountInches { get; set; }
    public DateTime CreatedOn { get; set; }
    public string WeatherType { get; set; }
    public string ZipCode { get; set; }
}