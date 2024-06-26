using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Accounts;

public record TransactionDetailsReward
{
    [JsonPropertyName("type")]
    public string? Type { get; init; }
    
    [JsonPropertyName("amount")] 
    public AmountModel? Amount { get; init; }
    
    [JsonPropertyName("points")]
    public string? Points { get; init; }
}