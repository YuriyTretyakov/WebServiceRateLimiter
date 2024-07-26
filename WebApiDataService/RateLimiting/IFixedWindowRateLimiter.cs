
namespace WebApiDataService.RateLimiting
{
    public interface IFixedWindowRateLimiter
    {
        TimeSpan Duration { get; }
        int RequestCount { get; }
        int TotalRequestCount { get; }
        DateTimeOffset WindowEnd { get; }
        DateTimeOffset WindowStart { get; }

        Task<bool> CanProcessRequestAsync();
    }
}