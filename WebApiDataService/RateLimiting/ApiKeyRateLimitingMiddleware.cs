using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Diagnostics;
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

            if (IsRequestRequireRateLimiting(context, out var requestHash, out var apiKey))
            {
                var (isApiKeyValid, rateLimit) = await _authorizationManager.IsKeyValidAsync(apiKey, CancellationToken.None);

                if (isApiKeyValid)
                {
                    var fixedRateLimiter = _memoryCache.GetOrCreate<FixedWindowRateLimiter>(
                        requestHash,
                        cacheItem =>
                        {
                            cacheItem.AbsoluteExpirationRelativeToNow = rateLimit.WindowDuration;
                            return new FixedWindowRateLimiter(rateLimit.RequestsCount, rateLimit.WindowDuration);
                        });

                    if (! await fixedRateLimiter.CanProcessRequestAsync())
                    {
                        _logger.LogWarning($"Api key {apiKey} exceeded its request limits");
                        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        return;
                    }
                    EnrichResponseHeadersWithCurentRateLimits(fixedRateLimiter, context);
                }
         
            }
            _logger.LogInformation($"{nameof(ApiKeyRateLimitingMiddleware)} InvokeAsync processed. It took {stopWatch.Elapsed}");
            await next.Invoke(context);
        }

        private bool IsRequestRequireRateLimiting(HttpContext context, out string requestHash, out string apiKey) 
        { 
            var requestPath = context.Request.Path.ToString();
            apiKey = string.Empty;
            requestHash = string.Empty;

            if (context.Request.Headers.TryGetValue(HttpConstants.ApiKeyHeaderName, out var apiKeyValues))
            {
                apiKey = apiKeyValues.FirstOrDefault(x => !string.IsNullOrEmpty(x));
                apiKey = apiKey ?? string.Empty;
                requestHash = new StringBuilder().Append(apiKey).Append(requestPath).ToString();
            }
            
            return true;
        }

        private void EnrichResponseHeadersWithCurentRateLimits(FixedWindowRateLimiter rateLimiter, HttpContext context)
        {
            context.Response.Headers.Add(nameof(rateLimiter.RequestCount), rateLimiter.RequestCount.ToString());
            context.Response.Headers.Add(nameof(rateLimiter.WindowEnd), rateLimiter.WindowEnd.ToString());
            context.Response.Headers.Add(nameof(rateLimiter.TotalRequestCount), rateLimiter.TotalRequestCount.ToString());
        }
    }
}
