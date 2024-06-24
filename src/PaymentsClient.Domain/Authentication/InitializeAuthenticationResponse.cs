using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Authentication;

public record InitializeAuthenticationResponse
{
    [JsonPropertyName("authUrl")] 
    public string AuthUrl { get; init; } = string.Empty;
    
    [JsonPropertyName("sessionId")]
    public string SessionId { get; init; } = string.Empty;
}