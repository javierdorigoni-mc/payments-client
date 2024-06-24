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
    public CardDetailsModel? Card { get; init; }

    [JsonPropertyName("bbanParsed")]
    public BbanParsed? BbanParsed { get; set; }
}

public record CardDetailsModel
{
    [JsonPropertyName("maskedPan")]
    public string? MaskedPanNumber { get; init; }
    
    [JsonPropertyName("cardHolder")]
    public string? CardHolder { get; init; }
    
    [JsonPropertyName("expireYear")]
    public int? ExpireYear { get; init; }
    
    [JsonPropertyName("expireMonth")]
    public int? ExpireMonth { get; init; }
}