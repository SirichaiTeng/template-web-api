using template_web_api.Models.Entities;

namespace template_web_api.Interfaces.IRepositories;

public interface IWeatherRepository
{
    Task<IEnumerable<WeatherForecast>> GetAllAsync();
    Task<WeatherForecast> GetByIdAsync(int id);
    Task<IEnumerable<WeatherForecast>> GetByCityAsync(string city);
    Task<WeatherForecast> CreateAsync(WeatherForecast weather);
    Task<WeatherForecast> UpdateAsync(WeatherForecast weather);
    Task<bool> DeleteAsync(int id);
}
