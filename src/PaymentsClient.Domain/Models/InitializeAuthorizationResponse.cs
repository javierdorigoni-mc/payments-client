namespace PaymentsClient.Domain.Models;

public record InitializeAuthorizationResponse(Uri AuthUrl, string SessionId);