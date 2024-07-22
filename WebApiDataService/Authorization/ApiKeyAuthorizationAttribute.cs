using Microsoft.AspNetCore.Mvc;

namespace WebApiDataService.Authorization
{
    public class ApiKeyAuthorizationAttribute : ServiceFilterAttribute
    {
        public ApiKeyAuthorizationAttribute()
            : base(typeof(ApiKeyAuthorizationFilter))
        {
        }
    }
}
