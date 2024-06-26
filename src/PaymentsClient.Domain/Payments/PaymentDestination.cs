using PaymentsClient.Domain.Accounts;

namespace PaymentsClient.Domain.Payments;

public record PaymentDestination(
    AccountBbanDetails Bban, 
    string? Iban,
    string? OwnAccount,
    string? InpaymentForm,
    string? Name,
    string? Address);