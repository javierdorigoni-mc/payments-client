using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Accounts;

public record AccountDetailsModel
{       
    [JsonPropertyName("bbanType")]
    public string? BbanType { get; init; }

    [JsonPropertyName("bban")]
    public string? Bban { get; init; }

    [JsonPropertyName("iban")]
    public string? Iban { get; init; }

    [JsonPropertyName("card")]
    public AccountCardDetailsModel? Card { get; init; }

    [JsonPropertyName("bbanParsed")]
    public AccountBbanDetails? BbanDetails { get; set; }
}