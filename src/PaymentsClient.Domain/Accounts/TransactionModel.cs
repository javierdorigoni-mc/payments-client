using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Accounts;

public record TransactionModel
{
    [JsonPropertyName("id")] 
    public string? Id { get; init; }
    
    [JsonPropertyName("idSchema")] 
    public string? IdSchema { get; init; }
    
    [JsonPropertyName("date")] 
    public DateTimeOffset? TransactionDate { get; init; }

    [JsonPropertyName("creationTime")] 
    public DateTimeOffset? CreationTime { get; init; }
    
    [JsonPropertyName("text")]
    public string? Text { get; init; }
    
    [JsonPropertyName("originalText")]
    public string? OriginalText { get; init; }
    
    [JsonPropertyName("details")] 
    public TransactionDetailsModel? Details { get; init; }
    
    [JsonPropertyName("category")]
    public string? Category { get; init; }

    [JsonPropertyName("ammount")] 
    public AmmountModel? TransactionAmmount { get; init; }
    
    [JsonPropertyName("balance")] 
    public AmmountModel? AccountBalance { get; init; }
    
    [JsonPropertyName("type")]
    public string? Type { get; init; }
    
    [JsonPropertyName("state")]
    [EnumDataType(typeof(TransactionState))]
    public string? State { get; init; }
}