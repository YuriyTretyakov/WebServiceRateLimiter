using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiDataService.DataLayer.Auth.Models
{
    [Table("Clients")]
    internal class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AdditionalInformation { get; set; }
        public ApiKey? ApiKey { get; set; }
    }
}
