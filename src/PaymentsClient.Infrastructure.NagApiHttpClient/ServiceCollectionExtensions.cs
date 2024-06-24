using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentsClient.Domain;
using Polly;
using Polly.Extensions.Http;

namespace PaymentsClient.Infrastructure.NagApiHttpClient;

public static class ServiceCollectionExtensions
{
    public static void AddNagApiHttpClientServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHttpClient<INagApiClientService, NagApiHttpClientService>(
                client =>
                {
                    client.BaseAddress = new Uri(configuration.GetValue<string>("NagApi:BaseUri") 
                                                 ?? throw new ArgumentException("Null NagApi Http Client Base Uri configuration"));
                    client.DefaultRequestHeaders.Add("X-Client-Id", configuration.GetValue<string>("NagApi:ClientId")
                                                                    ?? throw new ArgumentException("Null NagApi ClientId Configuration"));
                    client.DefaultRequestHeaders.Add("X-Client-Secret", configuration.GetValue<string>("NagApi:ClientSecret")
                                                                    ?? throw new ArgumentException("Null NagApi ClientSecret Configuration"));
                    client.Timeout = TimeSpan.FromSeconds(3);
                })
            .AddPolicyHandler(GetRetryPolicy());
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}