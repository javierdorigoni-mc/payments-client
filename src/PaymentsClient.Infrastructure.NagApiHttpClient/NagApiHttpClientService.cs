using PaymentsClient.Domain.Services;

namespace PaymentsClient.Infrastructure.NagApiHttpClient.Services;

public class NagApiHttpClientService : INagApiClientService
{
    private readonly HttpClient _httpClient;

    public NagApiHttpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public Task InitializeAuthenticationAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}