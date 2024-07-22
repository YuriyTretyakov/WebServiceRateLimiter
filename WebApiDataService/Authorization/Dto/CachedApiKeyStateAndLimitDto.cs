namespace WebApiDataService.Authorization.Dto
{
    internal  class CachedApiKeyStateAndLimitDto
    {
        internal ApiKeyStateDto State { get; set; }
        internal LimitsDto Limit { get; set; }
    }
}
