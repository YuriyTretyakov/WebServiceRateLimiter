using Microsoft.EntityFrameworkCore;
using WebApiDataService.Authorization.Dto;
using WebApiDataService.DataLayer.Auth.Models;

namespace WebApiDataService.DataLayer.Auth
{
    internal class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly AuthorizationDbContext _dbcontext;

        public AuthorizationRepository(AuthorizationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<ClientDto> AddClientAsync(ClientDto client, CancellationToken cancellationToken)
        {
            var dbClient = new Client
            {
                AdditionalInformation = client.AdditionalInformation,
                Name = client.Name
            };

            await _dbcontext.Clients.AddAsync(dbClient, cancellationToken);
            await _dbcontext.SaveChangesAsync(cancellationToken);
            client.Id = dbClient.Id;

            return client;
        }

        public async Task<ApiKeyDto> GetKeyAsync(string key, CancellationToken cancellationToken)
        {
            var dbKey = await _dbcontext.ApiKeys.SingleOrDefaultAsync(x => x.ApiKeyValue == key, cancellationToken);

            return dbKey is null ? null : new ApiKeyDto
            {
                Id = dbKey.Id,
                ApiKeyValue = dbKey.ApiKeyValue,
                PaymentPlan = dbKey.PaymentPlan,
                State = dbKey.State,
            };
        }

        public async Task<ApiKeyDto> UpsertApiKeyAsync(ApiKeyDto apiKey, CancellationToken cancellationToken)
        {
            var existingKey = await _dbcontext
                .ApiKeys
                .SingleOrDefaultAsync(x => x.Id == apiKey.Id);

            if (existingKey is null)
            {
                var dbKey = new ApiKey
                {
                    ApiKeyValue = apiKey.ApiKeyValue,
                    PaymentPlan = apiKey.PaymentPlan ?? PaymentPlanTypeDto.Basic,
                    UpdatedAt = DateTimeOffset.UtcNow,
                    State = apiKey.State
                };

                await _dbcontext.ApiKeys.AddAsync(dbKey, cancellationToken);
                await _dbcontext.SaveChangesAsync(cancellationToken);
                apiKey.Id = dbKey.Id;
            }
            else
            {
                existingKey.State = apiKey.State;
                existingKey.UpdatedAt = DateTimeOffset.UtcNow;
                existingKey.PaymentPlan = apiKey.PaymentPlan ?? existingKey.PaymentPlan;
                await _dbcontext.SaveChangesAsync(cancellationToken);
                apiKey.Id = existingKey.Id;
                apiKey.ApiKeyValue = existingKey.ApiKeyValue;
                apiKey.PaymentPlan = existingKey.PaymentPlan;
            }
            

            return apiKey;
        }

        public async Task<bool> AssignApiKeyToClientAsync(Guid apiKey, Guid clientId, CancellationToken cancellationToken)
        {
            var existingKey = await _dbcontext
                .ApiKeys
                .SingleOrDefaultAsync(x => x.Id == apiKey, cancellationToken);

            if (existingKey is null)
            {
                return false;
            }

            var existingClient = await _dbcontext
                .Clients
                .SingleOrDefaultAsync(x => x.Id == clientId, cancellationToken);

            if (existingClient is null)
            {
                return false;
            }

            existingClient.ApiKey = existingKey;

            return await _dbcontext.SaveChangesAsync(cancellationToken) > 0;

        }

        public LimitsDto GetLimitsByPlan(PaymentPlanTypeDto paymentPlanTypeDto)
        {
            switch (paymentPlanTypeDto)
            {
                case PaymentPlanTypeDto.Basic: return new LimitsDto { RequestsCount = 10 , WindowDuration = TimeSpan.FromSeconds(10)};
                case PaymentPlanTypeDto.Advanced: return new LimitsDto { RequestsCount = 100, WindowDuration = TimeSpan.FromSeconds(10) };
                case PaymentPlanTypeDto.VIP: return new LimitsDto { RequestsCount = 1000, WindowDuration = TimeSpan.FromSeconds(10) };

                default:
                    throw new NotImplementedException(nameof(paymentPlanTypeDto));
            }
        }

        public async Task<IReadOnlyCollection<ClientDto>> GetAllClientsAsync(CancellationToken cancellationToken)
        {
            return await _dbcontext.Clients.AsNoTracking().Select(
                x=> new ClientDto {
                    Id = x.Id,
                    Name = x.Name,
                    AdditionalInformation = x.AdditionalInformation })
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<ApiKeyDto>> GetAllApiKeysAsync(CancellationToken cancellationToken)
        {
            return await _dbcontext.ApiKeys.AsNoTracking().Select(
                x => new ApiKeyDto
                {
                    Id = x.Id,
                    ApiKeyValue = x.ApiKeyValue,
                    PaymentPlan = x.PaymentPlan,
                    State = x.State,

                })
                .ToListAsync(cancellationToken);
        }
    }
}
