using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Accounts;

public record TransactionDetailsModel
{       
    [JsonPropertyName("message")]
    public string? Message { get; init; }

    [JsonPropertyName("valueDate")]
    public DateTimeOffset? ValueDate { get; init; }
    
    [JsonPropertyName("executionDate")]
    public DateTimeOffset? ExecutionDate { get; init; }
    
    [JsonPropertyName("reward")]
    public TransactionDetailReward? Reward { get; init; }
    
    [JsonPropertyName("source")]
    public TransactionDetailAccount? Source { get; init; }
    
    [JsonPropertyName("destination")]
    public TransactionDetailAccount? Destination { get; init; }
    
    [JsonPropertyName("identifiers")]
    public TransactionIdentifiers? Identifiers { get; init; }
    
    [JsonPropertyName("routing")]
    public string? Routing { get; init; }
    
    [JsonPropertyName("currencyConversion")]
    public string? CurrencyConversion { get; init; }
}

public record TransactionDetailAccount
{
    [JsonPropertyName("account")]
    public AccountDetailsModel? AccountDetails { get; init; }
    
    [JsonPropertyName("name")]
    public string? Name { get; init; }
    
    [JsonPropertyName("address")]
    public string? Address { get; init; }
    
    [JsonPropertyName("merchantCategoryCode")]
    public string? MerchantCategoryCode { get; init; }
    
    [JsonPropertyName("merchantCategoryName")]
    public string? MerchantCategoryName { get; init; }
}

public record TransactionIdentifiers
{   
    [JsonPropertyName("reference")]
    public string? Reference { get; init; }
    
    [JsonPropertyName("document")]
    public string? Document { get; init; }
    
    [JsonPropertyName("sequenceNumber")]
    public string? SequenceNumber { get; init; }
    
    [JsonPropertyName("terminal")]
    public string? Terminal { get; init; }
    
    [JsonPropertyName("creditorReference")]
    public string? CreditorReference { get; init; }
    
    [JsonPropertyName("endToEndId")]
    public string? EndToEndId { get; init; }
    
    [JsonPropertyName("finnishReference")]
    public string? FinnishReference { get; init; }
    
    [JsonPropertyName("finnishArchiveId")]
    public string? FinnishArchiveId { get; init; }
    
    [JsonPropertyName("norwegianKid")]
    public string? NorwegianKid { get; init; }
}

public record TransactionDetailReward
{
    [JsonPropertyName("type")]
    public string? Type { get; init; }
    
    [JsonPropertyName("ammount")] 
    public AmmountModel? Ammount { get; init; }
    
    [JsonPropertyName("points")]
    public string? Points { get; init; }
}