namespace PaymentsClient.Domain.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly INagApiClientService _nagApiClientService;

    public AuthenticationService(INagApiClientService nagApiClientService)
    {
        _nagApiClientService = nagApiClientService;
    }
    
    public async Task<Result<InitializeAuthenticationResponse>> InitializeAuthenticationAsync(InitializeAuthenticationRequest request, CancellationToken cancellationToken = default)
    {
        return await _nagApiClientService.InitializeAuthenticationAsync(request, cancellationToken);
    }

    public async Task<Result<CompleteAuthenticationResponse>> CompleteAuthenticationAsync(CompleteAuthenticationRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return Result<CompleteAuthenticationResponse>.Failure("Invalid exchange token code");
        }

        return await _nagApiClientService.ExchangeTokenAsync(request, cancellationToken);
    }
}