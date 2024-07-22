namespace WebApiDataService.DataLayer.Weather.Models
{
    public class ForecastReporter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<WeatherForecast> ReportedForecasts { get; set; }

        public ICollection<ForecastReporterEvent> ReportedEvents { get; set; }
    }
}
