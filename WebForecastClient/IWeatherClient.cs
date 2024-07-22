using WebApiExploration.Dto;

namespace WebForecastClient
{
    public interface IWeatherClient
    {
        Task<WeatherForecastDto> GetForecastAsync(Guid id, CancellationToken cancellationToken);
    }
}