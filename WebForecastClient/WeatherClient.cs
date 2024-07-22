
using WebApiExploration.Dto;

namespace WebForecastClient
{
    public class WeatherClient : IWeatherClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<WeatherClient> _logger;

        public WeatherClient(IHttpClientFactory clientFactory, ILogger<WeatherClient> logger)
        {
            _client = clientFactory.CreateClient("WeatherClient");
            _logger = logger;
        }

        public async Task<WeatherForecastDto> GetForecastAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _client.GetFromJsonAsync<WeatherForecastDto>($"/WeatherForecast/getbuggy/{id}", cancellationToken);
            return result;
        }
    }
}
