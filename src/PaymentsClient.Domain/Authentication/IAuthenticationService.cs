namespace PaymentsClient.Domain.Authentication;

public interface IAuthenticationService
{
    Task<Result<InitializeAuthenticationResponse>> InitializeAuthenticationAsync(InitializeAuthenticationRequest request, CancellationToken cancellationToken = default);
    Task<Result<CompleteAuthenticationResponse>> CompleteAuthenticationAsync(CompleteAuthenticationRequest request, CancellationToken cancellationToken = default);
}