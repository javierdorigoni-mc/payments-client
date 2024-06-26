namespace PaymentsClient.Domain.Payments;

public record CreatePaymentRequest(
    string UserHash,
    CreatePaymentRequestDetails Request,
    string RedirectUrl,
    bool IssuePayerToken = true,
    string? PayerToken = null);