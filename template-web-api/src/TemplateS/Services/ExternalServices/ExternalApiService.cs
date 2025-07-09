using Microsoft.Extensions.Options;
using System.Text.Json;
using template_web_api.Common.AppSettings;
using template_web_api.Interfaces.IExternalServices;
using template_web_api.Models.Entities;

namespace template_web_api.Services.ExternalServices;

public class ExternalApiService : IExternalApiService
{
    private readonly HttpClient _httpClient;
    private readonly ExternalApiSettings _settings;

    public ExternalApiService(HttpClient httpClient, IOptions<ExternalApiSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<WeatherForecast> GetWeatherDataAsync(string city)
    {
        try
        {
            var url = $"{_settings.WeatherApiUrl}?q={city}&appid={_settings.WeatherApiKey}&units=metric";
            var response = await _httpClient.GetStringAsync(url);

            // Parse API response (simplified)
            var weatherData = JsonSerializer.Deserialize<dynamic>(response);

            return new WeatherForecast
            {
                City = city,
                Date = DateTime.Now,
                TemperatureC = (int)weatherData.main.temp,
                Summary = weatherData.weather[0].main,
                Description = weatherData.weather[0].description,
                Humidity = weatherData.main.humidity,
                WindSpeed = weatherData.wind.speed
            };
        }
        catch (Exception)
        {
            // Return mock data if API fails
            return new WeatherForecast
            {
                City = city,
                Date = DateTime.Now,
                TemperatureC = 25,
                Summary = "Sunny",
                Description = "Clear sky",
                Humidity = 60,
                WindSpeed = 5.5
            };
        }
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        try
        {
            var response = await _httpClient.GetStringAsync(_settings.ProductApiUrl);
            var products = JsonSerializer.Deserialize<IEnumerable<Product>>(response);
            return products;
        }
        catch (Exception)
        {
            // Return mock data if API fails
            return new List<Product>
                {
                    new Product
                    {
                        Id = 1,
                        Name = "Weather Station",
                        Price = 299.99m,
                        Description = "Digital weather monitoring device",
                        Category = "Electronics",
                        Stock = 10
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Temperature Sensor",
                        Price = 49.99m,
                        Description = "Wireless temperature sensor",
                        Category = "Electronics",
                        Stock = 25
                    }
                };
        }
    }

}
