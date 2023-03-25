﻿using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Configuration;
using MicroWeather.DataLoader.Models;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text.Json;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var servicesConfig = configuration.GetSection("Services");

var tempServiceConfig = JsonSerializer.Deserialize<ServiceConfig>(servicesConfig.GetSection("Temperature").Value);

var precipitationServiceConfig = JsonSerializer.Deserialize<ServiceConfig>(servicesConfig.GetSection("Precipitation").Value);

var zipCodes = new List<string>
{
    "73026",
    "68104",
    "04401",
    "32821",
    "19717"
};

Console.WriteLine("Starting the data load");

var rand = new Random();

var temperatureHttpClient = new HttpClient();
temperatureHttpClient.BaseAddress = new Uri($"http://{tempServiceConfig.Host}:{tempServiceConfig.Port}");

var precipitationHttpClient = new HttpClient();
precipitationHttpClient.BaseAddress = new Uri($"http://{precipitationServiceConfig.Host}:{precipitationServiceConfig.Port}");

foreach(var zip in zipCodes)
{
    Console.WriteLine($"Processing the {zip} zip code");
    var from = DateTime.Now.AddYears(-2);
    
    for(var day = from.Date; day.Date <= DateTime.Now; day = day.AddDays(1))
    {
        var temps = PostTemperature(zip, day);
        PostPrecipitation(temps[0], zip, day);
    }
}


void PostPrecipitation(int lowTemp, string zip, DateTime day)
{
    var isPrecip = rand.Next(2) < 1;
    var weatherType = "none";
    PrecipitationModel precipitationModel;
    if (isPrecip)
    {
        var precipInches = rand.Next(1, 16);
        if (lowTemp < 0)
            weatherType = "snow";
        else
            weatherType = "rain";

        precipitationModel = new PrecipitationModel()
        {
            AmmountInches = precipInches,
            WeatherType = weatherType,
            ZipCode = zip,
            CreatedOn = day
        };
    } 
    else
    {
        precipitationModel = new PrecipitationModel()
        {
            AmmountInches = 0,
            WeatherType = weatherType,
            ZipCode = zip,
            CreatedOn = day
        };
    }

    var precipitationResponse = precipitationHttpClient?.PostAsJsonAsync("observation", precipitationModel).Result;


    if (precipitationResponse.IsSuccessStatusCode)
        Console.WriteLine(
            $"Posted Precipitation: Date {day:d}" +
            $"Zip: {zip}" +
            $"Type: {weatherType}" +
            $"Amount: {precipitationModel.AmmountInches}");
}

List<int> PostTemperature(string zip, DateTime day)
{
    var t1 = rand.Next(-30, 30);
    var t2 = rand.Next(-30, 30);
    if (t1 > t2)
        (t1, t2) = (t2, t1);

    var hTemps = new List<int> { t1, t2 };
    var temperatureModel = new TemperatureModel()
    {
        TemperatureHighC = hTemps[1],
        TemperatureLowC = hTemps[0],
        ZipCode = zip,
        CreatedOn = day
    };

    var temperatureResponse = precipitationHttpClient?.PostAsJsonAsync("observation", temperatureModel).Result;


    if (temperatureResponse.IsSuccessStatusCode)
        Console.WriteLine(
            $"Posted Precipitation: Date {day:d}" +
            $"Zip: {zip}" +
            $"Lo: {temperatureModel.TemperatureLowC}" +
            $"Hi: {temperatureModel.TemperatureHighC}");

    return hTemps;
}