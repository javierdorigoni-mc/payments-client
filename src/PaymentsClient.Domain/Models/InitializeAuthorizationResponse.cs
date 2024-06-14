using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Models;

public record InitializeAuthorizationResponse
{
    [JsonPropertyName("authUrl")]
    public string AuthUrl { get; init; }
    
    [JsonPropertyName("sessionId")]
    public string SessionId { get; init; }
}