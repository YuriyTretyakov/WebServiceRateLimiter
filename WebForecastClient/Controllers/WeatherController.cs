using Microsoft.AspNetCore.Mvc;

namespace WebForecastClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
      
        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherClient _client;

        public WeatherController(ILogger<WeatherController> logger, IWeatherClient client)
        {
            _logger = logger;
            _client = client;
            
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
        {
           var result = await  _client.GetForecastAsync(id, cancellationToken);
           return Ok(result);
        }
    }
}
