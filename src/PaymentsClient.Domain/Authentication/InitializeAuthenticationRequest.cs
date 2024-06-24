namespace PaymentsClient.Domain.Authentication;

public record InitializeAuthenticationRequest(string UserHash, string RedirectUrl);