using PaymentsClient.Domain.Accounts;
using PaymentsClient.Domain.Authentication;
using PaymentsClient.Domain.Payments;

namespace PaymentsClient.Domain;

public interface INagApiClientService
{
    Task<Result<InitializeAuthenticationResponse>> InitializeAuthenticationAsync(InitializeAuthenticationRequest request, CancellationToken cancellationToken = default);
    Task<Result<CompleteAuthenticationResponse>> ExchangeTokenAsync(CompleteAuthenticationRequest completeAuthenticationRequest, CancellationToken cancellationToken = default);
    Task<Result<GetAccountsResponse>> GetAccountsAsync(GetAccountsRequest request, CancellationToken cancellationToken = default);
    Task<Result<GetTransactionsResponse>> GetTransactionsAsync(GetTransactionsRequest request, CancellationToken cancellationToken = default);
    Task<Result<CreatePaymentResponse>> CreatePaymentAsync(CreatePaymentRequest request, CancellationToken cancellationToken = default);
    Task<Result<RefreshPaymentStatusResponse>> RefreshPaymentStatusAsync(RefreshPaymentStatusRequest request, CancellationToken cancellationToken = default);
}