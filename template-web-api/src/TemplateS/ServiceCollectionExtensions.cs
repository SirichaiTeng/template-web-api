using Microsoft.EntityFrameworkCore;
using template_web_api.Common.AppSettings;
using template_web_api.Common.Data;
using template_web_api.Interfaces;
using template_web_api.Interfaces.IExternalServices;
using template_web_api.Interfaces.IRepositories;
using template_web_api.Repositories;
using template_web_api.Services;
using template_web_api.Services.ExternalServices;

namespace template_web_api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<WeatherDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Configuration
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.Configure<ExternalApiSettings>(configuration.GetSection("ExternalApiSettings"));

        // Repositories
        services.AddScoped<IWeatherRepository, WeatherRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        // Services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IExternalApiService, ExternalApiService>();

        // HTTP Client
        services.AddHttpClient<IExternalApiService, ExternalApiService>();

        // Caching
        services.AddMemoryCache();

        return services;
    }
}

