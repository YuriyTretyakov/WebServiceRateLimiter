using WebApiDataService.Authorization.Dto;

namespace WebApiDataService.RequestModels
{
    public class CreateNewApiKeyRequest
    {
        public  PaymentPlanTypeDto PaymentPlan { get; set; }
    }
}
