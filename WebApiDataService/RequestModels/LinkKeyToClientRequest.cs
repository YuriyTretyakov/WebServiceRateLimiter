namespace WebApiDataService.RequestModels
{
    public class LinkKeyToClientRequest
    {
        public Guid ApiKeyId { get; set; }
        public Guid ClientId { get; set; }
    }
}
