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
                    client.BaseAddress = nagApiSettings.BaseUri ?? throw new ArgumentNullException(nameof(nagApiSettings.BaseUri), "Missing NagApi Http Client Base Uri configuration");
                    client.DefaultRequestHeaders.Add("X-Client-Id", nagApiSettings.ClientId ?? throw new ArgumentNullException(nameof(nagApiSettings.ClientId), "Missing NagApi ClientId Configuration"));
                    client.DefaultRequestHeaders.Add("X-Client-Secret", nagApiSettings.ClientSecret ?? throw new ArgumentNullException(nameof(nagApiSettings.ClientSecret), "Missing NagApi ClientSecret Configuration"));
                    client.Timeout = TimeSpan.FromSeconds(nagApiSettings.TimeOutInSeconds ?? throw new ArgumentNullException(nameof(nagApiSettings.TimeOutInSeconds), "Missing NagApi TimeOutInSeconds Configuration"));
                })
            .AddPolicyHandler(
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
    }
}