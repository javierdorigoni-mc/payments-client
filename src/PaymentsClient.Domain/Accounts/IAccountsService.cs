namespace PaymentsClient.Domain.Accounts;

public interface IAccountsService
{
    Task<Result<GetAccountsResponse>> GetAccountsAsync(GetAccountsRequest request, CancellationToken cancellationToken = default);
    Task<Result<GetTransactionsResponse>> GetTransactionsAsync(GetTransactionsRequest request, CancellationToken cancellationToken = default);
}