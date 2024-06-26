using System.Text.Json.Serialization;
using PaymentsClient.Domain.Accounts;

namespace PaymentsClient.Domain.Payments;

public record PaymentDetailsSourceModel
{    
    [JsonPropertyName("bban")]
    public AccountBbanDetails? BbanDetails { get; init; }    
    
    [JsonPropertyName("iban")]
    public PaymentIbanDetails? Iban { get; init; }
    
    [JsonPropertyName("ownAccount")]
    public string? OwnAccount { get; init; }
    
    [JsonPropertyName("inpaymentForm")]
    public string? InpaymentForm { get; init; }
    
    [JsonPropertyName("name")]
    public string? Name { get; init; }
    
    [JsonPropertyName("address")]
    public string? Address { get; init; }
}