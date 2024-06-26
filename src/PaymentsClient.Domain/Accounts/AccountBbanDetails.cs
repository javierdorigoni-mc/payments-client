using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Accounts;

public record AccountBbanDetails
{
    [JsonPropertyName("bankCode")]
    public string? BankCode { get; init; }

    [JsonPropertyName("accountNumber")]
    public string? AccountNumber { get; init; }
}