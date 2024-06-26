using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Accounts;

public record AccountCardDetailsModel
{
    [JsonPropertyName("maskedPan")]
    public string? MaskedPanNumber { get; init; }
    
    [JsonPropertyName("cardHolder")]
    public string? CardHolder { get; init; }
    
    [JsonPropertyName("expireYear")]
    public int? ExpireYear { get; init; }
    
    [JsonPropertyName("expireMonth")]
    public int? ExpireMonth { get; init; }
}