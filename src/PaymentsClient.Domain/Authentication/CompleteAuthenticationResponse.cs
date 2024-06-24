using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Authentication;

public record CompleteAuthenticationResponse
{
    [JsonPropertyName("session")] 
    public SessionModel? Session { get; init; }
    
    [JsonPropertyName("login")]
    public LoginModel? Login { get; init; }
    
    [JsonPropertyName("providerId")]
    public string? ProviderId { get; init; }
}

public record SessionModel
{   
    [JsonPropertyName("expires")]
    public DateTimeOffset? Expires { get; init; }
    
    [JsonPropertyName("accessToken")]
    public string? AccessToken { get; init; }
}

public class LoginModel
{
    [JsonPropertyName("providerId")]
    public string? ProviderId { get; init; }
    
    [JsonPropertyName("expires")]
    public DateTimeOffset? Expires { get; init; }
    
    [JsonPropertyName("loginToken")]
    public string? LoginToken { get; init; }
    
    [JsonPropertyName("supportsUnattended")]
    public bool? SupportsUnattended { get; init; }
    
    [JsonPropertyName("label")]
    public string? Label { get; init; }
    
    [JsonPropertyName("subjectId")]
    public string? SubjectId { get; init; }
}
