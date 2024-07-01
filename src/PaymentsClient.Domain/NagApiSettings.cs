namespace PaymentsClient.Domain;

public record NagApiSettings
{
    public Uri? BaseUri { get; set; }
    public string? ClientId { get; set; }    
    public string? ClientSecret { get; set; }
    public double? TimeOutInSeconds { get; set; }
}