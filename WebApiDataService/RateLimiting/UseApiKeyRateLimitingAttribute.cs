using Microsoft.AspNetCore.Mvc;

namespace WebApiDataService.RateLimiting
{
    public class UseApiKeyRateLimitingAttribute : MiddlewareFilterAttribute
    {
        public UseApiKeyRateLimitingAttribute() : base(typeof(UseApiKeyRateLimitingAttribute))
        {
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ApiKeyRateLimitingMiddleware>();
        }
    }
}
