using PaymentsClient.Domain.Models;

namespace PaymentsClient.Domain.Services;

public interface INagApiClientService
{
    Task<Result<InitializeAuthorizationResponse>> InitializeAuthenticationAsync(InitializeAuthorizationRequest request, CancellationToken cancellationToken = default);
}