using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Accounts;

public record TransactionDetailsAccount
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