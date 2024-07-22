using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using WebApiDataService.DataLayer.Auth;


namespace WebApiDataService.Authorization
{
    public static class AuthorizationRegistrationExtension
    {
        public static IServiceCollection AddApiKeyAuthorization(this IServiceCollection services)
        {
            services.AddDbContext<AuthorizationDbContext>(opt => opt.UseSqlServer("Data Source=localhost,1433;Initial Catalog=AuthorizationDb;User id=sa;Password=Pa$w0rd!;TrustServerCertificate=True"));
            services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
            services.AddScoped<IApiKeyAuthorizationManager, ApiKeyAuthorizationManager>();
            services.AddScoped<ApiKeyAuthorizationFilter>();
            services.AddHttpContextAccessor();

            return services;
        }

    }
}
