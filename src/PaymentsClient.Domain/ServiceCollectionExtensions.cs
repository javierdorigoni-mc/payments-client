using Microsoft.Extensions.DependencyInjection;
using PaymentsClient.Domain.Accounts;
using PaymentsClient.Domain.Authentication;
using PaymentsClient.Domain.Payments;

namespace PaymentsClient.Domain;

public static class ServiceCollectionExtensions
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        services
            .AddTransient<IAuthenticationService, AuthenticationService>()
            .AddTransient<IAccountsService, AccountsService>()
            .AddTransient<IPaymentsService, PaymentsService>();
    }
}