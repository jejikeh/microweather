namespace MicroWeather.Report.Config;
public class ServiceConfig
{
    public required string Protocol { get; set; }
    public required string Host { get; set; }
    public required string Port { get; set; }

    public ServiceConfig(string protocol,string host, string port)
    {
        Protocol = protocol;
        Port = port;
        Host = host;
    }
}
