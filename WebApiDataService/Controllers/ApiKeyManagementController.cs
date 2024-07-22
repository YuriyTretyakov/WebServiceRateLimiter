using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using WebApiDataService.DataLayer.Auth;
using WebApiDataService.RequestModels;

namespace WebApiDataService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiKeyManagementController : ControllerBase
    {
        private readonly ILogger<ApiKeyManagementController> _logger;
        private readonly IApiKeyAuthorizationManager _authorizationService;
        private readonly IAuthorizationRepository _authorizationRepository;

        public ApiKeyManagementController(
            ILogger<ApiKeyManagementController> logger,
            IApiKeyAuthorizationManager authorizationService,
            IAuthorizationRepository authorizationRepository)
        {
            _logger = logger;
            _authorizationService = authorizationService;
            _authorizationRepository = authorizationRepository;
        }

        [HttpPost("generate-key")]
        public async Task<IActionResult> GenerateApiKey([FromBody] CreateNewApiKeyRequest createNewApiKeyRequest,  CancellationToken cancellationToken)
        {
           var key = await _authorizationService.AddKeyAsync(
                createNewApiKeyRequest.PaymentPlan,
                Authorization.Dto.ApiKeyStateDto.Created,
                cancellationToken);

            return Ok(key);
        }

        [HttpPost("add-key-to-client")]
        public async Task<IActionResult> AssociateClientWithKey([FromBody] LinkKeyToClientRequest keyToClientRequest, CancellationToken cancellationToken)
        {
            await _authorizationService.AssignKeyAsync(keyToClientRequest.ApiKeyId, keyToClientRequest.ClientId, cancellationToken);
            return Ok();
        }

        [HttpPost("update-state")]
        public async Task<IActionResult> AssociateClientWithKey([FromBody] UpdateApiKeyRequest updateApiKeyRequest, CancellationToken cancellationToken)
        {
            await _authorizationService.UpdateStateAsync(updateApiKeyRequest.Id, updateApiKeyRequest.NewState, cancellationToken);
            return Ok();
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllApiKeys(CancellationToken cancellationToken)
        {
           var keys =  await _authorizationRepository.GetAllApiKeysAsync(cancellationToken);
           return Ok(keys);
        }

    }
}
