using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Accounts;

public record TransactionDetailsIdentifiers
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