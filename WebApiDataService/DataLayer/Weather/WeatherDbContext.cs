using Microsoft.EntityFrameworkCore;
using WebApiDataService.DataLayer.Weather.Models;

namespace WebApiDataService.DataLayer.Weather
{
    public class WeatherDbContext : DbContext
    {
        public DbSet<WeatherForecast> Forecasts { get; set; }
        public DbSet<ForecastReporterEvent> ReportedEvents { get; set; }

        public DbSet<ForecastReporter> Reporters { get; set; }



        public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherForecast>().HasIndex(x => x.Id).IsUnique();
            modelBuilder.Entity<WeatherForecast>().HasIndex(x => new { x.Date, x.Summary });

            modelBuilder.Entity<ForecastReporterEvent>().HasIndex(x => x.Id).IsUnique();

            modelBuilder.Entity<ForecastReporter>().HasIndex(x => x.Id).IsUnique();
            modelBuilder.Entity<ForecastReporter>().HasIndex(x => x.Name).IsUnique();


            modelBuilder
                .Entity<ForecastReporterEvent>()
                .HasOne(x => x.Reporter)
                .WithMany(x => x.ReportedEvents)
                .HasForeignKey(x => x.ReporterId).OnDelete(DeleteBehavior.Restrict);


            modelBuilder
                .Entity<ForecastReporterEvent>()
                .HasOne(x => x.Forecast)
                .WithOne(x => x.Event)
                .HasForeignKey<ForecastReporterEvent>(x => x.ForecastId).OnDelete(DeleteBehavior.Restrict);

            //modelBuilder
            //    .Entity<WeatherForecast>()
            //    .HasOne(x => x.Reporter)
            //    .WithMany(x => x.ReportedForecasts)
            //    .HasForeignKey(x => x.ReporterId);


            //modelBuilder
            //   .Entity<ForecastReporterEvent>()
            //   .HasOne(x => x.Forecast)
            //   .WithOne(x => x.Event)
            //   .HasForeignKey<WeatherForecast>(x => x.Id);
        }
    }
}
