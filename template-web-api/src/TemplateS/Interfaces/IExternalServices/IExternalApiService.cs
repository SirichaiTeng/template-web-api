using template_web_api.Models.Entities;

namespace template_web_api.Interfaces.IExternalServices;

public interface IExternalApiService
{
    Task<WeatherForecast> GetWeatherDataAsync(string city);
    Task<IEnumerable<Product>> GetProductsAsync();
}
