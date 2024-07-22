using System.ComponentModel.DataAnnotations.Schema;
using WebApiDataService.Authorization.Dto;

namespace WebApiDataService.DataLayer.Auth.Models
{
    [Table("ApiKeys")]
    internal class ApiKey
    {
        public Guid Id { get; set; }
        public string ApiKeyValue { get; set; }
        public PaymentPlanTypeDto PaymentPlan { get; set; }
        public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }
        public ApiKeyStateDto State { get; set; }
        public Guid? ClientId { get; set; }
    }
}
