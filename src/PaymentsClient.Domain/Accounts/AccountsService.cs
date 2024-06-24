namespace PaymentsClient.Domain.Accounts;

public class AccountsService : IAccountsService
{
    private readonly INagApiClientService _nagApiClientService;

    public AccountsService(INagApiClientService nagApiClientService)
    {
        _nagApiClientService = nagApiClientService;
    }
    
    public async Task<Result<GetAccountsResponse>> GetAccountsAsync(GetAccountsRequest request, CancellationToken cancellationToken = default)
    {
        return await _nagApiClientService.GetAccountsAsync(request, cancellationToken);
    }
    
    public async Task<Result<GetTransactionsResponse>> GetTransactionsAsync(GetTransactionsRequest request, CancellationToken cancellationToken = default)
    {
        return await _nagApiClientService.GetTransactionsAsync(request, cancellationToken);
    }
}