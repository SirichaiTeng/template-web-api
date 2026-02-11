using OriginalExample.Data;
using OriginalExample.Interfaces.IData;
using OriginalExample.Interfaces.IServices;
using OriginalExample.Services;
using Serilog;

namespace OriginalExample;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var app = CreateApp(args);

        if (!args.Contains("test"))
        {
            await app.RunAsync();
        }
    }

    public static WebApplication CreateApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        RegisterServices(builder.Configuration, builder.Services);
        // Add services to the container.
        builder.Host.UseSerilog((ctx, lc) =>
                    lc.ReadFrom.Configuration(ctx.Configuration)
                );
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        return app;
    }

    private static void RegisterServices(IConfiguration configuration,IServiceCollection services)
    {
        services.AddSingleton<Serilog.ILogger>(Log.Logger);
        services.AddScoped<IDapperWarpper, DapperWarpper>();
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        services.AddScoped<ITokenService, TokenService>();

        var map = new Dictionary<string, string>()
        {
            {"TokenApi","TokenApi:BaseAddress" },
            {"","" }

        };

        foreach (var (name,key) in map)
        {
            services.AddHttpClient(name, client =>
            {
                client.BaseAddress = new Uri(configuration[key] ?? "");
            });
        }
    }
}
