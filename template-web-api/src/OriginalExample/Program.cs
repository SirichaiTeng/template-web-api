
using OriginalExample.Data;
using OriginalExample.Interfaces.IData;

namespace OriginalExample;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        RegisterServices(builder.Configuration,builder.Services);
        // Add services to the container.

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

        app.Run();
    }

    private static void RegisterServices(IConfiguration configuration,IServiceCollection services)
    {
        services.AddScoped<IDapperWarpper, DapperWarpper>();
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
    }
}
