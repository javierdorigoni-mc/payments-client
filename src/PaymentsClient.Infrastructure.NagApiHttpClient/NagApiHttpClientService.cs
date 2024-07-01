using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using PaymentsClient.Domain;
using PaymentsClient.Domain.Accounts;
using PaymentsClient.Domain.Authentication;
using PaymentsClient.Domain.Payments;

namespace PaymentsClient.Infrastructure.NagApiHttpClient;

public class NagApiHttpClientService : INagApiClientService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<NagApiHttpClientService> _logger;

    public NagApiHttpClientService(
        HttpClient httpClient,
        ILogger<NagApiHttpClientService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
    public async Task<Result<InitializeAuthenticationResponse>> InitializeAuthenticationAsync(
        InitializeAuthenticationRequest request, 
        CancellationToken cancellationToken = default)
    {
        var httpRequestMessage = HttpRequestMessageBuilder
            .Create()
            .WithHttpMethod(HttpMethod.Post)
            .WithRequestUri("v1/authentication/initialize")
            .WithJsonSerializedContent(request)
            .Build();
        
        return await ExecuteHttpRequestAsync<InitializeAuthenticationResponse>(httpRequestMessage, cancellationToken);
    }
    
    public async Task<Result<CompleteAuthenticationResponse>> ExchangeTokenAsync(
        CompleteAuthenticationRequest request, 
        CancellationToken cancellationToken = default)
    {
        var httpRequestMessage = HttpRequestMessageBuilder
            .Create()
            .WithHttpMethod(HttpMethod.Post)
            .WithRequestUri("v1/authentication/tokens")
            .WithJsonSerializedContent(request)
            .Build();   
        
        return await ExecuteHttpRequestAsync<CompleteAuthenticationResponse>(httpRequestMessage, cancellationToken);
    }
    
    public async Task<Result<GetAccountsResponse>> GetAccountsAsync(
        GetAccountsRequest request, 
        CancellationToken cancellationToken = default)
    {        
        var httpRequestMessage = HttpRequestMessageBuilder
            .Create()
            .WithHttpMethod(HttpMethod.Get)
            .WithRequestUri("v2/accounts")
            .WithAuthorizationBearerTokenHeader(request.AccessToken)
            .Build();
        
        return await ExecuteHttpRequestAsync<GetAccountsResponse>(httpRequestMessage, cancellationToken);
    }

    public async Task<Result<GetTransactionsResponse>> GetTransactionsAsync(
        GetTransactionsRequest request,
        CancellationToken cancellationToken = default)
    {
        var httpRequestMessage = HttpRequestMessageBuilder
            .Create()
            .WithHttpMethod(HttpMethod.Get)
            .WithRequestUri($"v2/accounts/{request.AccountId}/transactions")
            .WithOptionalQueryStringParameter("fromDate", request.FromDate)
            .WithOptionalQueryStringParameter("withDetails", request.WithDetails?.ToString().ToLowerInvariant())
            .WithAuthorizationBearerTokenHeader(request.AccessToken)
            .Build();

        return await ExecuteHttpRequestAsync<GetTransactionsResponse>(httpRequestMessage, cancellationToken);
    }

    public async Task<Result<CreatePaymentResponse>> CreatePaymentAsync(
        CreatePaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        var httpRequestMessage = HttpRequestMessageBuilder
            .Create()
            .WithHttpMethod(HttpMethod.Post)
            .WithRequestUri("v1/payments/create")
            .WithJsonSerializedContent(request)
            .Build();

        return await ExecuteHttpRequestAsync<CreatePaymentResponse>(httpRequestMessage, cancellationToken);
    }

    public async Task<Result<RefreshPaymentStatusResponse>> RefreshPaymentStatusAsync(
        RefreshPaymentStatusRequest request, 
        CancellationToken cancellationToken = default)
    {
        var httpRequestMessage = HttpRequestMessageBuilder
            .Create()
            .WithHttpMethod(HttpMethod.Post)
            .WithRequestUri($"v1/payments/{request.PaymentId}/refresh-status")
            .Build();

        return await ExecuteHttpRequestAsync<RefreshPaymentStatusResponse>(httpRequestMessage, cancellationToken);
    }

    private async Task<Result<TResponse>> ExecuteHttpRequestAsync<TResponse>(
        HttpRequestMessage requestMessage, 
        CancellationToken cancellationToken = default) where TResponse : class
    {
        try
        {
            var httpResponse = await _httpClient.SendAsync(requestMessage, cancellationToken);

            httpResponse.EnsureSuccessStatusCode();

            var stringContentResponse = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            
            return Result<TResponse>.Success(
                JsonSerializer.Deserialize<TResponse>(stringContentResponse)
                ?? throw new ArgumentNullException("HttpResponse","Unable to deserialize Http Response Body"));
        }
        catch (HttpRequestException hrex) when (hrex.StatusCode == HttpStatusCode.Forbidden)
        {
            _logger.LogError(hrex.Message);
            return Result<TResponse>.Failure(KnownResultErrors.Forbidden);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result<TResponse>.Failure("There is an issue with your request, please verify the logs.");
        }
    }
}