using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Weather.Domain.Common.Constants;
using WebApiDataService.DataLayer.Auth;

namespace WebApiDataService.Authorization
{
    public class ApiKeyAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly IApiKeyAuthorizationManager _service;

        public ApiKeyAuthorizationFilter(IApiKeyAuthorizationManager service)
        {
            _service = service;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(HttpConstants.ApiKeyHeaderName, out var currentApiKey))
            {
                context.Result = new UnauthorizedResult();
            }

            var result  = await _service.IsKeyValidAsync(currentApiKey);

            if (!result)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}

