using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Retry;
using WebForecastClient;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHttpClient(
            "WeatherClient",
            x =>
            {
                x.BaseAddress = new Uri("http://localhost:5003");
            })
            .AddTransientHttpErrorPolicy(x=>
        
            x.WaitAndRetryAsync(5, attempts =>
                TimeSpan.FromSeconds(Math.Pow(2, attempts))))
            .AddTransientHttpErrorPolicy(x=>
                x.AdvancedCircuitBreakerAsync<HttpResponseMessage>(
                    0.5,
                    TimeSpan.FromSeconds(10),
                    2,
                    TimeSpan.FromSeconds(10),
                  //  (this PolicyBuilder<TResult> policyBuilder, double failureThreshold, TimeSpan samplingDuration, int minimumThroughput,
                  //  TimeSpan durationOfBreak, Action<DelegateResult<TResult>, CircuitState, TimeSpan, Context> onBreak, Action<Context> onReset, Action onHalfOpen)
    
                    (_, state,_,_)=>
                    {
                        if (state == CircuitState.Open)
                        {
                           
                        }
                    },
                    new Action <Context>(x => { }),
                    new Action(() => { })));



        builder.Services.AddTransient<IWeatherClient, WeatherClient>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        app.UseFileServer();
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.Run();

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return  Policy<HttpResponseMessage>
                .Handle<ApplicationException>()
                .OrResult(x=>x.StatusCode is System.Net.HttpStatusCode.InternalServerError or  System.Net.HttpStatusCode.ServiceUnavailable)
                .WaitAndRetryAsync(5, attempts=>
                TimeSpan.FromSeconds(Math.Pow(2, attempts)));
        }

        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return Policy<HttpResponseMessage>
                            .Handle<ApplicationException>()
                            .OrResult(x => x.StatusCode is System.Net.HttpStatusCode.InternalServerError or System.Net.HttpStatusCode.ServiceUnavailable)
                            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }

        

        //static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        //{
        //    return HttpPolicyExtensions
        //        .HandleTransientHttpError()
        //        .WaitAndRetryAsync(5, _ => TimeSpan.FromSeconds(2));
        //}
    }
}