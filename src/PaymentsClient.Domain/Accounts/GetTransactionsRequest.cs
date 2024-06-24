namespace PaymentsClient.Domain.Accounts;

public record GetTransactionsRequest(string AccessToken, string AccountId, string? FromDate, bool? WithDetails = false);