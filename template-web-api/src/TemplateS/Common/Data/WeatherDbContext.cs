using Microsoft.EntityFrameworkCore;
using template_web_api.Models.Entities;

namespace template_web_api.Common.Data;

public class WeatherDbContext : DbContext
{
    public WeatherDbContext(DbContext options) : base(options) { }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed data
        modelBuilder.Entity<WeatherForecast>().HasData(
            new WeatherForecast
            {
                Id = 1,
                Date = DateTime.Now.AddDays(1),
                TemperatureC = 25,
                Summary = "Warm",
                City = "Bangkok"
            },
            new WeatherForecast
            {
                Id = 2,
                Date = DateTime.Now.AddDays(2),
                TemperatureC = 30,
                Summary = "Hot",
                City = "Bangkok"
            }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Weather Station",
                Price = 299.99m,
                Description = "Digital weather monitoring device"
            },
            new Product
            {
                Id = 2,
                Name = "Temperature Sensor",
                Price = 49.99m,
                Description = "Wireless temperature sensor"
            }
        );
    }
}
