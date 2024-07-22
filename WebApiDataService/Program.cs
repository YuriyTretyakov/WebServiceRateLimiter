using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using WebApiDataService.Authorization;
using WebApiDataService.DataLayer.Auth;
using WebApiDataService.DataLayer.Weather;
using WebApiDataService.RateLimiting;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
           .Build();

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

       

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        builder.Services
            .AddDbContext<WeatherDbContext>(
            opt => opt
            .UseSqlServer("Data Source=localhost,1433;Initial Catalog=WeatherForecastDb;User id=sa;Password=Pa$w0rd!;TrustServerCertificate=True"));

        builder.Services.AddScoped<WeatherForecastRepository>();
        builder.Services.AddApiKeyRateLimiter();
        builder.Services.AddMemoryCache();

        builder.Services.AddApiKeyAuthorization();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
                db.Database.Migrate();

                var authDb = scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();
                authDb.Database.Migrate();
            }
        }


        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

       // app.UseSerilogRequestLogging();


        app.Run();
    }
}