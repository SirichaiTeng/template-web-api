using Microsoft.EntityFrameworkCore;
using template_web_api.Common.Data;
using template_web_api.Interfaces.IRepositories;
using template_web_api.Models.Entities;

namespace template_web_api.Repositories;

public class WeatherRepository : IWeatherRepository
{
    private readonly WeatherDbContext _context;

    public WeatherRepository(WeatherDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WeatherForecast>> GetAllAsync()
    {
        return await _context.WeatherForecasts
            .OrderByDescending(w => w.Date)
            .ToListAsync();
    }

    public async Task<WeatherForecast> GetByIdAsync(int id)
    {
        return await _context.WeatherForecasts.FindAsync(id);
    }

    public async Task<IEnumerable<WeatherForecast>> GetByCityAsync(string city)
    {
        return await _context.WeatherForecasts
            .Where(w => w.City.ToLower() == city.ToLower())
            .OrderByDescending(w => w.Date)
            .ToListAsync();
    }

    public async Task<WeatherForecast> CreateAsync(WeatherForecast weather)
    {
        _context.WeatherForecasts.Add(weather);
        await _context.SaveChangesAsync();
        return weather;
    }

    public async Task<WeatherForecast> UpdateAsync(WeatherForecast weather)
    {
        _context.Entry(weather).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return weather;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var weather = await _context.WeatherForecasts.FindAsync(id);
        if (weather == null) return false;

        _context.WeatherForecasts.Remove(weather);
        await _context.SaveChangesAsync();
        return true;
    }
}
