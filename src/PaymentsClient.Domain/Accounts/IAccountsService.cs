namespace PaymentsClient.Domain.Accounts;

public interface IAccountsService
{
    Task<Result<GetAccountsResponse>> GetAccountsAsync(GetAccountsRequest request, CancellationToken cancellationToken = default);
}