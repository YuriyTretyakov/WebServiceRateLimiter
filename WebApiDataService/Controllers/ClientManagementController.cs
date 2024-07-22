using Microsoft.AspNetCore.Mvc;
using WebApiDataService.Authorization.Dto;
using WebApiDataService.DataLayer.Auth;
using WebApiDataService.RequestModels;

namespace WebApiDataService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientManagementController:ControllerBase
    {
        private readonly ILogger<ClientManagementController> _logger;
        private readonly IAuthorizationRepository _authorizationRepository;

        public ClientManagementController(
            ILogger<ClientManagementController> logger,
            IAuthorizationRepository authorizationRepository)
        {
            _logger = logger;
            _authorizationRepository = authorizationRepository;
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateNewUser([FromBody] CreateNewUserRequest createNewUserRequest, CancellationToken cancellationToken)
        {
            var newClinetDto = new ClientDto
            {
                Name = createNewUserRequest.Name,
                AdditionalInformation = createNewUserRequest.AdditionalInformation
            };

            var newClient = await _authorizationRepository.AddClientAsync(newClinetDto, cancellationToken);
            return Ok(newClient);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            var allClients = await _authorizationRepository.GetAllClientsAsync(cancellationToken);
            return Ok(allClients);
        }
    }
}
