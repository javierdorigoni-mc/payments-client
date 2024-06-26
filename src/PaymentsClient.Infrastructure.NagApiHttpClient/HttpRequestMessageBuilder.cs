using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace PaymentsClient.Infrastructure.NagApiHttpClient;

public class HttpRequestMessageBuilder
{   
    private HttpMethod? _httpMethod;
    private string _requestUri = string.Empty;
    private HttpContent? _httpContent;
    private AuthenticationHeaderValue? _authenticationHeader;

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
    
    public HttpRequestMessageBuilder WithOptionalQueryStringParameter(string queryStringName, string? queryStringValue)
    {
        if (string.IsNullOrWhiteSpace(_requestUri) || string.IsNullOrWhiteSpace(queryStringValue))
        {
            return this;
        }

        var urlStringBuilder = new StringBuilder(2000);
        urlStringBuilder.Append(_requestUri);
        urlStringBuilder.Append(_requestUri.Contains('?') ? '&' : '?');
        urlStringBuilder.Append(queryStringName);
        urlStringBuilder.Append('=');
        urlStringBuilder.Append(queryStringValue);
        _requestUri = urlStringBuilder.ToString();
        
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