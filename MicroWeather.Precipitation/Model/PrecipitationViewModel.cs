namespace MicroWeather.Precipitation.Model;

public class PrecipitationViewModel
{
    public decimal AmountInches { get; set; }
    public DateTime CreateOn { get; set; }
    public string WeatherType { get; set; }
    public string ZipCode { get; set; }
}