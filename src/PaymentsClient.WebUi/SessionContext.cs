namespace PaymentsClient.WebUi;

public record SessionContext
{
    public string? UserHash { get; set; }
    public string? AccessToken { get; set; }
    public string? BankAccountCode { get; set; }
    public string? BankAccountNumber { get; set; }
}