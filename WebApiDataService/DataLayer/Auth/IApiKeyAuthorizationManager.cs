using WebApiDataService.Authorization.Dto;

namespace WebApiDataService.DataLayer.Auth
{
    public interface IApiKeyAuthorizationManager
    {
        Task<ApiKeyDto> AddKeyAsync(PaymentPlanTypeDto plan, ApiKeyStateDto state, CancellationToken cancellationToken);
        Task AssignKeyAsync(Guid keyId, Guid clientId, CancellationToken cancellationToken);
        Task DeleteKeyAsync(Guid apiKeyid, CancellationToken cancellationToken);
        ValueTask<bool> IsKeyValidAsync(string key, CancellationToken cancellationToken = default);
        ValueTask<LimitsDto> GetLimitsForApiKeyAsync(string key, CancellationToken cancellationToken = default);
        Task PauseKeyAsync(Guid apiKeyid, CancellationToken cancellationToken);
        Task UpdatePlanAsync(Guid keyId, PaymentPlanTypeDto newPlan, CancellationToken cancellationToken);
        Task UpdateStateAsync(Guid keyId, ApiKeyStateDto newState, CancellationToken cancellationToken);
    }
}