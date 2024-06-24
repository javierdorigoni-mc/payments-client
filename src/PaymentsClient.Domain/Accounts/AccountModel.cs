using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Accounts;

public record AccountModel
{
    [JsonPropertyName("id")] 
    public string? Id { get; init; }
    
    [JsonPropertyName("providerId")] 
    public string? ProviderId { get; init; }
    
    [JsonPropertyName("name")]
    public string? Name { get; init; }
    
    [JsonPropertyName("number")] 
    public AccountDetailsModel? AccountNumberDetails { get; init; }
    
    [JsonPropertyName("bookedBalance")] 
    public AmmountModel? BookedBalance { get; init; }
    
    [JsonPropertyName("availableBalance")] 
    public AmmountModel? AvailableBalance { get; init; }
    
    [JsonPropertyName("type")]
    public string? Type { get; init; }
    
    [JsonPropertyName("features")] 
    public Features? Features { get; init; }
}

public record BbanParsed
{
    [JsonPropertyName("bankCode")]
    public string? BankCode { get; init; }

    [JsonPropertyName("accountNumber")]
    public string? AccountNumber { get; init; }
}

public record Features
{
    [JsonPropertyName("queryable")] 
    public bool? Queryable { get; init; } = true;

    [JsonPropertyName("psdPaymentAccount")]
    public bool? PsdPaymentAccount { get; init; } = true;

    [JsonPropertyName("paymentFrom")] 
    public bool? PaymentFrom { get; init; } = true;

    [JsonPropertyName("paymentTo")]
    public bool? PaymentTo { get; init; } = true;
}