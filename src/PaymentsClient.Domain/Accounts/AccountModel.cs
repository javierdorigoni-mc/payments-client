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
    public AccountNumber? AccountNumber { get; init; }
    
    [JsonPropertyName("bookedBalance")] 
    public Balance? BookedBalance { get; init; }
    
    [JsonPropertyName("availableBalance")] 
    public Balance? AvailableBalance { get; init; }
    
    [JsonPropertyName("type")]
    public string? Type { get; init; }
    
    [JsonPropertyName("features")] 
    public Features? Features { get; init; }
}

public record AccountNumber
{       
    [JsonPropertyName("bbanType")]
    public string? BbanType { get; init; }

    [JsonPropertyName("bban")]
    public string? Bban { get; init; }

    [JsonPropertyName("iban")]
    public string? Iban { get; init; }

    [JsonPropertyName("card")]
    public string? Card { get; init; }

    [JsonPropertyName("bbanParsed")]
    public BbanParsed? BbanParsed { get; set; }
}    

public record BbanParsed
{
    [JsonPropertyName("bankCode")]
    public string? BankCode { get; init; }

    [JsonPropertyName("accountNumber")]
    public string? AccountNumber { get; init; }
}

public record Balance
{        
    [JsonPropertyName("value")]
    public double? Value { get; init; }

    [JsonPropertyName("currency")]
    public string? Currency { get; init; }
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