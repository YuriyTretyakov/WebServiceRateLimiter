
using WebApiExploration.Dto;

namespace Weather.Domain.Common.Dto
{
    public class AddForecastRequest
    {
        public Guid ReporterId { get; set; }
        public WeatherForecastDto Forecast { get; set; }
    }
}
