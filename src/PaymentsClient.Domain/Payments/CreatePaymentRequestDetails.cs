using PaymentsClient.Domain.Accounts;

namespace PaymentsClient.Domain.Payments;

public record CreatePaymentRequestDetails(PaymentDestination Destination, AmountModel Amount);