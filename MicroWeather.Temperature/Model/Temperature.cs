namespace MicroWeather.Temperature.Model;

public class Temperature 
{
    public int Id { get; set; }
    public Guid CreatedOn { get; set; }
    public decimal TempHighC { get; set; }
    public decimal TempLowC { get; set; }
    public string ZipCode { get; set; } = "undefined";
}