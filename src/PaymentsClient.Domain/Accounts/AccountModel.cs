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
    public AmountModel? BookedBalance { get; init; }
    
    [JsonPropertyName("availableBalance")] 
    public AmountModel? AvailableBalance { get; init; }
    
    [JsonPropertyName("type")]
    public string? Type { get; init; }
    
    [JsonPropertyName("features")] 
    public AccountFeatures? Features { get; init; }
}