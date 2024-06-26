namespace PaymentsClient.Domain.Payments;

public enum PaymentState
{
    Preparing = 1,
    ReadyForAuthorize,
    Authorizing,
    Pending,
    Succeeded,
    Failed,
    Cancelled
}