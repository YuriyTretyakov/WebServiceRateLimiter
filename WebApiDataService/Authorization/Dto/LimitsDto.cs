namespace WebApiDataService.Authorization.Dto
{
    public class LimitsDto
    {
        public int RequestsCount {  get; set; }
        public TimeSpan WindowDuration { get; set; }
    }
}
