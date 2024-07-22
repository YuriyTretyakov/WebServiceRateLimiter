namespace WebApiDataService.Authorization.Dto
{
    public class ApiKeyDto
    {
        public Guid Id { get; set; }
        public string ApiKeyValue { get; set; }
        public PaymentPlanTypeDto? PaymentPlan { get; set; }
        public ApiKeyStateDto State { get; set; }
    }
}
