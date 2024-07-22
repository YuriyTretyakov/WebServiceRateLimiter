namespace WebApiDataService.RequestModels
{
    public class CreateNewUserRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AdditionalInformation { get; set; }
    }
}
