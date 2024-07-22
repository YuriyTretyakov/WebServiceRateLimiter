using WebApiDataService.Authorization.Dto;

namespace WebApiDataService.DataLayer.Auth
{
    public interface IAuthorizationRepository
    {
        Task<ClientDto> AddClientAsync(ClientDto client, CancellationToken cancellationToken);
        Task<bool> AssignApiKeyToClientAsync(Guid apiKey, Guid clientId, CancellationToken cancellationToken);
        LimitsDto GetLimitsByPlan(PaymentPlanTypeDto paymentPlanTypeDto);
        Task<ApiKeyDto> UpsertApiKeyAsync(ApiKeyDto apiKey, CancellationToken cancellationToken);
        Task<ApiKeyDto> GetKeyAsync(string key, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<ClientDto>> GetAllClientsAsync(CancellationToken cancellationToken);
        Task<IReadOnlyCollection<ApiKeyDto>> GetAllApiKeysAsync(CancellationToken cancellationToken);
    }
}