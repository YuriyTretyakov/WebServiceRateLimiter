using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Net;
using System.Text;
using Weather.Domain.Common.Constants;
using WebApiDataService.DataLayer.Auth;

namespace WebApiDataService.RateLimiting
{
    public class ApiKeyRateLimitingMiddleware : IMiddleware
    {
        private readonly ILogger<ApiKeyRateLimitingMiddleware> _logger;
        private readonly IApiKeyAuthorizationManager _authorizationManager;
        private readonly IMemoryCache _memoryCache;

        public ApiKeyRateLimitingMiddleware(
            ILogger<ApiKeyRateLimitingMiddleware> logger,
            IApiKeyAuthorizationManager authorizationManager,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _authorizationManager = authorizationManager;
            _memoryCache = memoryCache;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var (requestHash, apiKey) = GetApiKeyAndRquestHash(context);

            if (apiKey is not null)
            {
                var fixedRateLimiter = await _memoryCache.GetOrCreateAsync<IFixedWindowRateLimiter>(
                    requestHash,
                    async cacheItem =>
                    {
                        var rateLimits = await _authorizationManager.GetLimitsForApiKeyAsync(apiKey, CancellationToken.None);
                        cacheItem.AbsoluteExpirationRelativeToNow = rateLimits.WindowDuration;
                        return new FixedWindowRateLimiter(rateLimits.RequestsCount, rateLimits.WindowDuration);
                    });

                if (!await fixedRateLimiter.CanProcessRequestAsync())
                {
                    _logger.LogWarning($"Api key {apiKey} exceeded its request limits");
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    return;
                }
                EnrichResponseWithCurentRateLimits(fixedRateLimiter, context);

            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.CompleteAsync();
            }
            _logger.LogInformation($"{nameof(ApiKeyRateLimitingMiddleware)} InvokeAsync processed. It took {stopWatch.Elapsed}");
            await next.Invoke(context);
        }

        private (string RequestHash, string? ApiKey) GetApiKeyAndRquestHash(HttpContext context) 
        { 
            var requestPath = context.Request.Path.ToString();
            string apiKey = null;
            var requestHash = string.Empty;

            if (context.Request.Headers.TryGetValue(HttpConstants.ApiKeyHeaderName, out var apiKeyValues))
            {
                apiKey = apiKeyValues.FirstOrDefault(x => !string.IsNullOrEmpty(x));
                apiKey = apiKey ?? string.Empty;
                requestHash = new StringBuilder().Append(apiKey).Append(requestPath).ToString();
            }
            
            return (requestHash, apiKey);
        }

        private void EnrichResponseWithCurentRateLimits(IFixedWindowRateLimiter rateLimiter, HttpContext context)
        {
            context.Response.Headers.Add(nameof(rateLimiter.RequestCount), rateLimiter.RequestCount.ToString());
            context.Response.Headers.Add(nameof(rateLimiter.WindowEnd), rateLimiter.WindowEnd.ToString());
            context.Response.Headers.Add(nameof(rateLimiter.TotalRequestCount), rateLimiter.TotalRequestCount.ToString());
        }
    }
}
