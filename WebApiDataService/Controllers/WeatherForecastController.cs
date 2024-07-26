using Microsoft.AspNetCore.Mvc;
using Weather.Domain.Common.Dto;
using WebApiDataService.Authorization;
using WebApiDataService.DataLayer.Weather;
using WebApiDataService.DataLayer.Weather.Models;
using WebApiDataService.RateLimiting;
using WebApiExploration.Dto;

namespace WebApiExploration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherForecastRepository _weatherForecastManager;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherForecastRepository weatherForecastManager)
        {
            _logger = logger;
            _weatherForecastManager = weatherForecastManager;
        }

        [UseApiKeyRateLimiting]
        [HttpGet("get-forecast")]
        public IEnumerable<WeatherForecast> Get(CancellationToken cancellationToken)
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTimeOffset.UtcNow.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("add-forecast")]
        public async Task<IActionResult> Save([FromBody] AddForecastRequest weatherForecast, CancellationToken cancellationToken)
        {
           var created = await _weatherForecastManager.AddForecastAsync(weatherForecast.ReporterId, weatherForecast.Forecast, cancellationToken);
            return Ok(created);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var forecast  = await _weatherForecastManager.GetForecastAsync(id, cancellationToken);
            return Ok(forecast);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var forecast = await _weatherForecastManager.GetAllAsync(cancellationToken);
            return Ok();
        }

        [HttpPost("get-by-datetime-range")]
        public async Task<IActionResult> GetByDateAndTime([FromBody] DateRangeRequestDto dateRangeRequest, CancellationToken cancellationToken)
        {
            var forecast = await _weatherForecastManager.GetByDateTimeRangeAsync(dateRangeRequest, cancellationToken);
            return Ok();
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] ForecastUpdateDto updateRequest, CancellationToken cancellationToken)
        {
            var forecast = await _weatherForecastManager.UpdateAsync(updateRequest.ValueToUpdate, updateRequest.NewValue, cancellationToken);
            return Ok();
        }

        [HttpGet("get-by-page/{pagenumber}/{count}")]
        public async Task<IActionResult> GetByPage(int pagenumber, int count, CancellationToken cancellationToken)
        {
            var forecast = await _weatherForecastManager.GetByPageAsync(pagenumber, count, cancellationToken);
            return Ok(forecast);
        }

        [HttpGet("getbuggy/{id}")]
        public async Task<IActionResult> GetBuggyById(Guid id, CancellationToken cancellationToken)
        {
            var forecast = await _weatherForecastManager.GetForecastAsync(id, cancellationToken);

            var r = new Random();

            return r.Next(2) ==1? Ok(forecast): StatusCode(500);
        }

        [HttpPost("add-reporter")]
        public async Task<IActionResult> AddReporter([FromBody] ReporterDto reporter, CancellationToken cancellationToken)
        {
            var reporterCreated = await _weatherForecastManager.AddReporterAsync(reporter, cancellationToken);
            return Ok(reporterCreated);
        }
    }
}
