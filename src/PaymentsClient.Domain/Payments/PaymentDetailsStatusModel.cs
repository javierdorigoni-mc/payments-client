using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Payments;

public record PaymentDetailsStatusModel
{   
    [JsonPropertyName("terminal")]
    public bool Terminal { get; init; }
    
    [JsonPropertyName("code")]    
    [EnumDataType(typeof(PaymentState))] 
    public string? Code { get; init; }
    
    [JsonPropertyName("codeV2")]    
    public string? CodeV2 { get; init; }
    
    [JsonPropertyName("lastUpdated")]
    public DateTimeOffset? LastUpdated { get; init; }
    
    [JsonPropertyName("details")]    
    public PaymentsStatusDetailsModel? Details { get; init; }
}