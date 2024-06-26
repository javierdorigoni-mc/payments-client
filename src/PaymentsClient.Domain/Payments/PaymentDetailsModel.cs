using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PaymentsClient.Domain.Accounts;

namespace PaymentsClient.Domain.Payments;

public record PaymentDetailsModel
{   
    [JsonPropertyName("paymentId")]
    public string? PaymentId { get; init; }
    
    [JsonPropertyName("userHash")]
    public string? UserHash { get; init; }
    
    [JsonPropertyName("providerId")]
    public string? ProviderId { get; init; }
    
    [JsonPropertyName("providerPaymentId")]
    public string? ProviderPaymentId { get; init; }

    [JsonPropertyName("request")]
    public PaymentDetailsRequestModel? Request { get; init; }
    
    [JsonPropertyName("status")]
    public PaymentDetailsStatusModel? Status { get; init; }
    
    [JsonPropertyName("state")]
    [EnumDataType(typeof(PaymentState))]
    public string? State { get; init; }
    
    [JsonPropertyName("created")]
    public DateTimeOffset? Created { get; init; }
    
    [JsonPropertyName("source")]
    public PaymentDetailsSourceModel? Source { get; init; }
    
    [JsonPropertyName("execution")]
    public PaymentDetailsExecutionModel? Execution { get; init; }
    
    [JsonPropertyName("message")]
    public string? Message { get; init; }
    
    [JsonPropertyName("transactionText")]
    public string? TransactionText { get; init; }

    [JsonPropertyName("identifiers")]
    public TransactionDetailsIdentifiers? Identifiers { get; init; }
}