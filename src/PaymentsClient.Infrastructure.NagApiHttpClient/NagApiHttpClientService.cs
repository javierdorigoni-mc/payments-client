using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentsClient.Domain.Models;
using PaymentsClient.Domain.Services;

namespace PaymentsClient.Infrastructure.NagApiHttpClient;

public class NagApiHttpClientService : INagApiClientService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<NagApiHttpClientService> _logger;
    private readonly IOptions<NagApiOptions> _nagApiOptions;

    public NagApiHttpClientService(
        HttpClient httpClient,
        IOptions<NagApiOptions> nagApiOptions,
        ILogger<NagApiHttpClientService> logger)
    {
        _httpClient = httpClient;
        _nagApiOptions = nagApiOptions;
        _logger = logger;
    }

    public async Task<Result<InitializeAuthorizationResponse>> InitializeAuthenticationAsync(InitializeAuthorizationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var httpRequest = CreateHttpRequestMessage(request, HttpMethod.Post, "v1/authentication/initialize");
            
            var httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken);

            httpResponse.EnsureSuccessStatusCode();

            var stringContentResponse = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            
            return Result<InitializeAuthorizationResponse>.Success(
                JsonSerializer.Deserialize<InitializeAuthorizationResponse>(stringContentResponse)
                ?? throw new ArgumentNullException("HttpResponse","Unable to deserialize Http Response Body"));
        }
        catch (Exception e)
        {
            return Result<InitializeAuthorizationResponse>.Failure(e.Message);
        }
    }

    private HttpRequestMessage CreateHttpRequestMessage<T>(T bodyRequest, HttpMethod httpMethod, string uriPath) 
        where T : class
    {
        var httpRequest = new HttpRequestMessage(httpMethod, uriPath)
        {
            Content = JsonContent.Create(bodyRequest)
        };
        
        return httpRequest;
    }
}