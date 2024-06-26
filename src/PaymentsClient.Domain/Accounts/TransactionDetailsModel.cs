using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Accounts;

public record TransactionDetailsModel
{       
    [JsonPropertyName("message")]
    public string? Message { get; init; }

    [JsonPropertyName("valueDate")]
    public DateTimeOffset? ValueDate { get; init; }
    
    [JsonPropertyName("executionDate")]
    public DateTimeOffset? ExecutionDate { get; init; }
    
    [JsonPropertyName("reward")]
    public TransactionDetailsReward? Reward { get; init; }
    
    [JsonPropertyName("source")]
    public TransactionDetailsAccount? Source { get; init; }
    
    [JsonPropertyName("destination")]
    public TransactionDetailsAccount? Destination { get; init; }
    
    [JsonPropertyName("identifiers")]
    public TransactionDetailsIdentifiers? Identifiers { get; init; }
    
    [JsonPropertyName("routing")]
    public string? Routing { get; init; }
    
    [JsonPropertyName("currencyConversion")]
    public string? CurrencyConversion { get; init; }
}