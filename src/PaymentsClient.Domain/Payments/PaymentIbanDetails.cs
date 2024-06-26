using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Payments;

public record PaymentIbanDetails
{
    [JsonPropertyName("ibanNumber")]
    public string? IbanNumber { get; init; }
}