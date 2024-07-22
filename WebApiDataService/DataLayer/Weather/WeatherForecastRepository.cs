using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using Weather.Domain.Common.Dto;
using WebApiDataService.DataLayer.Weather.Models;
using WebApiExploration.Dto;

namespace WebApiDataService.DataLayer.Weather
{
    public class WeatherForecastRepository
    {
        private readonly WeatherDbContext _dbContext;
        private readonly ILogger<WeatherForecastRepository> _logger;
        private readonly IMemoryCache _memoryCache;

        private const string AllForecastsCacheKey = "AllForecastsCacheKey";

        public WeatherForecastRepository(WeatherDbContext dbContext, ILogger<WeatherForecastRepository> logger, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<WeatherForecastDto> AddForecastAsync(Guid reporterId, WeatherForecastDto forecast, CancellationToken cancellationToken)
        {
            var reporter = await _dbContext.Reporters.SingleOrDefaultAsync(x => x.Id == reporterId);

            if (reporter == null)
            {
                return default;
            }

            var dbForecast = new WeatherForecast
            {
                Date = forecast.Date.Date,
                TemperatureC = forecast.TemperatureC,
                TemperatureF = forecast.TemperatureF,
                Summary = forecast.Summary,
                Reporter = reporter
            };

            var reportingEvent = new ForecastReporterEvent
            {
                Reporter = reporter,
                ReportedAt = DateTimeOffset.UtcNow,
                Forecast = dbForecast
            };




            var result = await _dbContext.Forecasts.AddAsync(dbForecast, cancellationToken);

            await _dbContext.ReportedEvents.AddAsync(reportingEvent, cancellationToken);


            await _dbContext.SaveChangesAsync(cancellationToken);

            forecast.Id = dbForecast.Id;

            return forecast;
        }

        public async Task<WeatherForecastDto> GetForecastAsync(Guid id, CancellationToken cancellationToken)
        {
            var forecast = await _dbContext.Forecasts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

            return new WeatherForecastDto
            {
                Id = forecast.Id,
                Date = forecast.Date,
                TemperatureC = forecast.TemperatureC,
                TemperatureF = forecast.TemperatureF,
                Summary = forecast.Summary
            };
        }

        public async ValueTask<IReadOnlyCollection<WeatherForecastDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            List<WeatherForecastDto> result = null;
            try
            {

                if (_memoryCache.TryGetValue(AllForecastsCacheKey, out result))
                {
                    return result;
                }

                var forecasts = await _dbContext.Forecasts.AsNoTracking().ToListAsync(cancellationToken);

                result = forecasts.Select(x => new WeatherForecastDto
                {
                    Id = x.Id,
                    Date = x.Date,
                    TemperatureC = x.TemperatureC,
                    TemperatureF = x.TemperatureF,
                    Summary = x.Summary
                }).ToList();

                _memoryCache.Set(AllForecastsCacheKey, result, DateTimeOffset.UtcNow.AddSeconds(10));

                return result;
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation($"GetAllAsync request duration {stopwatch.Elapsed} Count: {result?.Count ?? 0}");
            }
        }

        public async ValueTask<IReadOnlyCollection<WeatherForecastDto>> GetByDateTimeRangeAsync(DateRangeRequestDto dateRangeRequest, CancellationToken cancellationToken)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var after = dateRangeRequest.After;
            var before = dateRangeRequest.Before;

            var afterDateString = after.ToUnixTimeMilliseconds().ToString();
            var beforeDateString = before.ToUnixTimeMilliseconds().ToString();

            var memCacheKey = $"by_date_{afterDateString}_{beforeDateString}";

            List<WeatherForecastDto> result = null;
            try
            {

                if (_memoryCache.TryGetValue(memCacheKey, out result))
                {
                    return result;
                }

                var forecasts = await _dbContext
                    .Forecasts
                    .AsNoTracking()
                    .Where(x => x.Date > after && x.Date < before)
                    .ToListAsync(cancellationToken);

                result = forecasts.Select(x => new WeatherForecastDto
                {
                    Id = x.Id,
                    Date = x.Date,
                    TemperatureC = x.TemperatureC,
                    TemperatureF = x.TemperatureF,
                    Summary = x.Summary
                }).ToList();

                _memoryCache.Set(memCacheKey, result, DateTimeOffset.UtcNow.AddSeconds(10));

                return result;
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation($"GetByDateTimeRangeAsync request duration {stopwatch.Elapsed} Count: {result?.Count ?? 0}");
            }
        }

        public async Task<int> UpdateAsync(string oldValue, string newValue, CancellationToken cancellationToken)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            int result = 0;
            try
            {
                result = await _dbContext.Forecasts.Where(x => x.Summary == oldValue).ExecuteUpdateAsync(x => x.SetProperty(x => x.Summary, newValue), cancellationToken);
                return result;
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation($"UpdateAsync request duration {stopwatch.Elapsed} Updated Items: {result}");
            }
        }

        public async Task<IReadOnlyCollection<WeatherForecastDto>> GetByPageAsync(int pageNumber, int itemsPerPage, CancellationToken cancellationToken)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            List<WeatherForecastDto> result = null;
            try
            {
                var dbResult = await _dbContext.Forecasts.Skip(pageNumber * itemsPerPage).Take(itemsPerPage).ToListAsync(cancellationToken);

                result = dbResult.Select(x => new WeatherForecastDto
                {
                    Id = x.Id,
                    Date = x.Date,
                    TemperatureC = x.TemperatureC,
                    TemperatureF = x.TemperatureF,
                    Summary = x.Summary
                }).ToList();

                return result;
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation($"GetByPageAsync request duration {stopwatch.Elapsed}. Items: {result.Count}");
            }
        }

        internal async Task<ReporterDto> AddReporterAsync(ReporterDto reporter, CancellationToken cancellationToken)
        {
            var dbReporter = new ForecastReporter { Name = reporter.Name };
            await _dbContext.Reporters.AddAsync(dbReporter, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            reporter.Id = dbReporter.Id;
            return reporter;
        }

        internal async Task<ReporterDto> GetReportedForecastsByReporter(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}