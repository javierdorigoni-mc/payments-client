using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Accounts;

public record AmmountModel
{        
    [JsonPropertyName("value")]
    public double? Value { get; init; }

    [JsonPropertyName("currency")]
    public string? Currency { get; init; }
}