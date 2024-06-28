using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Payments;

public record PaymentsStatusDetailsModel
{
    [JsonPropertyName("providerStatusCode")]    
    public string? ProviderStatusCode { get; init; }
    
    [JsonPropertyName("reason")]    
    public string? Reason { get; init; }

    [JsonPropertyName("providerMessage")]    
    public string? ProviderMessage { get; init; }

    [JsonPropertyName("errorType")]    
    public string? ErrorType { get; init; }

    [JsonPropertyName("errorCode")]    
    public string? ErrorCode { get; init; }

    [JsonPropertyName("lastKnownStatus")]    
    public string? LastKnownStatus { get; init; }
}