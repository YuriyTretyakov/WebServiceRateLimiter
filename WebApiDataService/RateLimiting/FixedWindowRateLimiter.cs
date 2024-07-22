
namespace WebApiDataService.RateLimiting
{
    public class FixedWindowRateLimiter
    {
        public int TotalRequestCount { get; private set; }
        public int RequestCount { get; private set; }
        public DateTimeOffset WindowStart { get; private set; }
        public DateTimeOffset WindowEnd { get; private set; }
        public TimeSpan Duration  { get; private set; }

        private bool _isInitialised;

        private SemaphoreSlim _semaphore = new SemaphoreSlim(1,1);

        public async Task<bool> CanProcessRequestAsync()
        {
            try
            {
                await _semaphore.WaitAsync();

                if (!_isInitialised)
                {
                    Reset();
                }

                //New window started
                if (DateTimeOffset.UtcNow > WindowEnd)
                {
                    Reset();
                    RequestCount--;
                    return RequestCount >= 1;
                }
                //

                // previos window still in progress and there are tokens
                if (RequestCount >= 1)
                {
                    RequestCount--;
                    return RequestCount >= 1;
                } //No tokens
                else
                {
                    return false;
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public FixedWindowRateLimiter(int requestCount, TimeSpan duration)
        {
            Duration = duration;
            RequestCount = requestCount;
            TotalRequestCount = requestCount;
        }

        private void Reset()
        {
            WindowStart = DateTimeOffset.UtcNow;
            WindowEnd = WindowStart+Duration;
            RequestCount = TotalRequestCount;
            _isInitialised = true;
        }
    }
}
