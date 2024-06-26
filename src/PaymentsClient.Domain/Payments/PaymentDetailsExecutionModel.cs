using System.Text.Json.Serialization;
using PaymentsClient.Domain.Accounts;

namespace PaymentsClient.Domain.Payments;

public record PaymentDetailsExecutionModel
{    
    [JsonPropertyName("type")]
    public string? Type { get; init; }
    
    [JsonPropertyName("date")]
    public DateTimeOffset? Date { get; init; }
    
    [JsonPropertyName("fee")]
    public string? Fee { get; init; }
    
    [JsonPropertyName("feeAmount")]
    public AmountModel? FeeAmount { get; init; }
}