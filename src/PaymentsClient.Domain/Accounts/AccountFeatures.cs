using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Accounts;

public record AccountFeatures
{
    [JsonPropertyName("queryable")] 
    public bool? Queryable { get; init; } = true;

    [JsonPropertyName("psdPaymentAccount")]
    public bool? PsdPaymentAccount { get; init; } = true;

    [JsonPropertyName("paymentFrom")] 
    public bool? PaymentFrom { get; init; } = true;

    [JsonPropertyName("paymentTo")]
    public bool? PaymentTo { get; init; } = true;
}