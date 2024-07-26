using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using WebApiDataService.Authorization.Dto;
using WebApiDataService.DataLayer.Auth;

namespace WebApiDataService.Authorization
{
    internal class ApiKeyAuthorizationManager : IApiKeyAuthorizationManager
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IAuthorizationRepository _repository;

        public ApiKeyAuthorizationManager(IMemoryCache memoryCache, IAuthorizationRepository repository)
        {
            _memoryCache = memoryCache;
            _repository = repository;
        }

        public async Task<ApiKeyDto> AddKeyAsync(
            PaymentPlanTypeDto plan,
            ApiKeyStateDto state,
            CancellationToken cancellationToken)
        {
            var key = GenerateKey();
            var keyDto = new ApiKeyDto
            {
                ApiKeyValue = key,
                PaymentPlan = plan,
                State = state
            };

            await _repository.UpsertApiKeyAsync(keyDto, cancellationToken);

            UpdateCache(key, plan, state);
            return keyDto;
        }

        public async Task AssignKeyAsync(Guid keyId, Guid clientId, CancellationToken cancellationToken)
        {
            await _repository.AssignApiKeyToClientAsync(keyId, clientId, cancellationToken);
        }

        public async ValueTask<bool> IsKeyValidAsync(string key, CancellationToken cancellationToken)
        {
            bool result = false;
            
            if (string.IsNullOrWhiteSpace(key))
            {
                 return false;
            }

            if (_memoryCache.TryGetValue<CachedApiKeyStateAndLimitDto>(key, out var stateAndLimitDto))
            {
                if (stateAndLimitDto?.State == ApiKeyStateDto.Active)
                { 
                    result = true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
               var keyDto = await _repository.GetKeyAsync(key, cancellationToken);

                if (keyDto == null) 
                {
                    UpdateCache(key, PaymentPlanTypeDto.Basic, ApiKeyStateDto.Unknown);
                    result = false;
                }
                else
                {
                    UpdateCache(key, keyDto.PaymentPlan.Value, keyDto.State);
                    result = true;
                }
            }
            
            return result;
        }

        public async ValueTask<LimitsDto> GetLimitsForApiKeyAsync(string key, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return default;
            }

            LimitsDto limit = null;

            if (_memoryCache.TryGetValue<CachedApiKeyStateAndLimitDto>(key, out var stateAndLimitDto))
            {
                if (stateAndLimitDto?.State == ApiKeyStateDto.Active)
                {
                    limit = stateAndLimitDto.Limit;
                }
            }
            else
            {
                var keyDto = await _repository.GetKeyAsync(key, cancellationToken);

                if (keyDto == null)
                {
                    UpdateCache(key, PaymentPlanTypeDto.Basic, ApiKeyStateDto.Unknown);
                }
                else
                {
                    UpdateCache(key, keyDto.PaymentPlan.Value, keyDto.State);
                    limit = _repository.GetLimitsByPlan(keyDto.PaymentPlan.Value);
                }
            }

            return limit;
        }

        public async Task UpdatePlanAsync(Guid keyId, PaymentPlanTypeDto newPlan, CancellationToken cancellationToken)
        {
            var keyDto = new ApiKeyDto
            {
                Id = keyId,
                PaymentPlan = newPlan,
            };
            keyDto = await _repository.UpsertApiKeyAsync(keyDto, cancellationToken);

            UpdateCache(keyDto.ApiKeyValue, newPlan, keyDto.State);
        }

        public async Task UpdateStateAsync(Guid keyId, ApiKeyStateDto newState, CancellationToken cancellationToken)
        {
            var keyDto = new ApiKeyDto
            {
                Id = keyId,
                State = newState,
            };
            keyDto = await _repository.UpsertApiKeyAsync(keyDto, cancellationToken);

            UpdateCache(keyDto.ApiKeyValue, keyDto.PaymentPlan.Value, keyDto.State);
        }

        public async Task DeleteKeyAsync(Guid apiKeyid, CancellationToken cancellationToken)
        {
            var keyDto = new ApiKeyDto
            {
                Id = apiKeyid,
                State = ApiKeyStateDto.Deleted
            };
            await _repository.UpsertApiKeyAsync(keyDto, cancellationToken);

            UpdateCache(keyDto.ApiKeyValue, keyDto.PaymentPlan.Value, ApiKeyStateDto.Deleted);
        }

        public async Task PauseKeyAsync(Guid apiKeyid, CancellationToken cancellationToken)
        {
            var keyDto = new ApiKeyDto
            {
                Id = apiKeyid,
                State = ApiKeyStateDto.Paused
            };
            await _repository.UpsertApiKeyAsync(keyDto, cancellationToken);

            UpdateCache(keyDto.ApiKeyValue, keyDto.PaymentPlan.Value, ApiKeyStateDto.Paused);
        }

        private void UpdateCache(string key, PaymentPlanTypeDto plan, ApiKeyStateDto state)
        {
            var cachedKeyData = new CachedApiKeyStateAndLimitDto
            {
                State = state,
                Limit = _repository.GetLimitsByPlan(plan),
            };

            _memoryCache.Set(key, cachedKeyData, DateTimeOffset.UtcNow.AddHours(1));
        }

        private string GenerateKey()
        {
            var key = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
                generator.GetBytes(key);
            return Convert.ToBase64String(key);
        }
    }
}
