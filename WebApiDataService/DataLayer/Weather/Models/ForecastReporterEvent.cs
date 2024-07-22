
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiDataService.DataLayer.Weather.Models
{
    public class ForecastReporterEvent
    {
        public Guid Id { get; set; }
        public DateTimeOffset ReportedAt { get; set; }

        public ForecastReporter Reporter { get; set; }
        public WeatherForecast Forecast { get; set; }


        public Guid? ReporterId { get; set; }

        public Guid? ForecastId { get; set; }
    }
}
