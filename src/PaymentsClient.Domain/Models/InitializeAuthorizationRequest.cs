namespace PaymentsClient.Domain.Models;

public record InitializeAuthorizationRequest(string UserHash, string RedirectUrl);