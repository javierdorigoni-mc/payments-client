using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Payments;

public record CreatePaymentResponse
{
    [JsonPropertyName("redirectUrl")]
    public string? RedirectUrl { get; init; }
    
    [JsonPropertyName("paymentId")]
    public string? PaymentId { get; init; }
}