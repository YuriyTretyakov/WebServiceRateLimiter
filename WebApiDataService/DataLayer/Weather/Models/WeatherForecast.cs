using Microsoft.EntityFrameworkCore;

namespace WebApiDataService.DataLayer.Weather.Models
{
    public class WeatherForecast
    {
        public Guid Id { get; set; }
        public DateTimeOffset Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF { get; set; }

        public string? Summary { get; set; }

        public ForecastReporter Reporter { get; set; }

        public ForecastReporterEvent Event { get; set; }
    }
}
