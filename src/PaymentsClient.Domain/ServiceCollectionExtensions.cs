using Microsoft.Extensions.DependencyInjection;
using PaymentsClient.Domain.Authentication;

namespace PaymentsClient.Domain;

public static class ServiceCollectionExtensions
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<IAuthenticationService, AuthenticationService>();
    }
}