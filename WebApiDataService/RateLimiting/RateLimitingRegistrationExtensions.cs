
namespace WebApiDataService.RateLimiting
{
    public static class RateLimitingRegistrationExtensions
    {
        public static IServiceCollection AddApiKeyRateLimiter(this IServiceCollection services)
        {
            services.AddScoped<ApiKeyRateLimitingMiddleware>();
            return services;
        }
    }
}
