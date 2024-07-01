using Microsoft.Extensions.DependencyInjection;
using PaymentsClient.Domain;
using Polly;
using Polly.Extensions.Http;

namespace PaymentsClient.Infrastructure.NagApiHttpClient;

public static class ServiceCollectionExtensions
{
    public static void AddNagApiHttpClientServices(this IServiceCollection services, NagApiSettings nagApiSettings)
    {
        services
            .AddHttpClient<INagApiClientService, NagApiHttpClientService>(
                client =>
                {
                    client.BaseAddress = nagApiSettings.BaseUri ?? throw new ArgumentException("Missing NagApi Http Client Base Uri configuration");
                    client.DefaultRequestHeaders.Add("X-Client-Id", nagApiSettings.ClientId ?? throw new ArgumentException("Missing NagApi ClientId Configuration"));
                    client.DefaultRequestHeaders.Add("X-Client-Secret", nagApiSettings.ClientSecret ?? throw new ArgumentException("Missing NagApi ClientSecret Configuration"));
                    client.Timeout = TimeSpan.FromSeconds(nagApiSettings.TimeOutInSeconds);
                })
            .AddPolicyHandler(
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
    }
}