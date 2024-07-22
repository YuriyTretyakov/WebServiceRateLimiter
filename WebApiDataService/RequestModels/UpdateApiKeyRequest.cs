using WebApiDataService.Authorization.Dto;

namespace WebApiDataService.RequestModels
{
    public class UpdateApiKeyRequest
    {
        public Guid Id { get; set; }
        public ApiKeyStateDto NewState { get; set; }
    }
}
