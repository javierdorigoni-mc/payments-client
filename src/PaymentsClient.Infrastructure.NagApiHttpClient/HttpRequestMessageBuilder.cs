using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace PaymentsClient.Infrastructure.NagApiHttpClient;

public class HttpRequestMessageBuilder
{   
    private HttpMethod? _httpMethod = null;
    private string _requestUri = string.Empty;
    private HttpContent? _httpContent = null;
    private AuthenticationHeaderValue? _authenticationHeader = null;

    private HttpRequestMessageBuilder() { }

    public static HttpRequestMessageBuilder Create()
    {
        return new HttpRequestMessageBuilder();
    }

    public HttpRequestMessageBuilder WithHttpMethod(HttpMethod httpMethod)
    {
        _httpMethod = httpMethod;
        return this;
    }
    
    public HttpRequestMessageBuilder WithRequestUri(string requestUri)
    {
        _requestUri = requestUri;
        return this;
    }
    
    public HttpRequestMessageBuilder WithJsonSerializedContent<T>(T bodyRequest) where T : class
    {
        _httpContent = JsonContent.Create(bodyRequest);
        return this;
    }
    
    public HttpRequestMessageBuilder WithAuthorizationBearerTokenHeader(string bearerToken)
    {
        _authenticationHeader = new AuthenticationHeaderValue("Bearer", bearerToken);
        return this;
    }

    public HttpRequestMessage Build()
    {
        return new HttpRequestMessage(_httpMethod ?? HttpMethod.Post, _requestUri)
        {
            Content = _httpContent,
            Headers = { Authorization = _authenticationHeader }
        };
    }
}