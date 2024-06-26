using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Payments;

public record RefreshPaymentStatusResponse
{    
    [JsonPropertyName("payment")]
    public PaymentDetailsModel PaymentDetails { get; init; }
}