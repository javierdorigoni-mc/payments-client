using System.Text.Json.Serialization;
using PaymentsClient.Domain.Accounts;

namespace PaymentsClient.Domain.Payments;

public record PaymentDetailsRequestModel
{
    [JsonPropertyName("sourceAccountId")]
    public string? SourceAccountId { get; init; }
    
    [JsonPropertyName("destination")]
    public PaymentDetailsSourceModel Destination { get; init; }

    [JsonPropertyName("amount")]
    public AmountModel Amount { get; init; }

    [JsonPropertyName("execution")]
    public string? Execution { get; init; }
    
    [JsonPropertyName("message")]
    public string? Message { get; init; }
    
    [JsonPropertyName("transactionText")]
    public string? TransactionText { get; init; }
    
    [JsonPropertyName("endToEndId")]
    public string? EndToEndId { get; init; }   
    
    [JsonPropertyName("reference")]
    public string? Reference { get; init; }
    
    [JsonPropertyName("identifiers")]
    public TransactionDetailsIdentifiers? Identifiers { get; init; }
    
    [JsonPropertyName("paymentMethod")]
    public string? PaymentMethod { get; init; }
}