using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Accounts;

public record GetAccountsResponse
{
    [JsonPropertyName("accounts")] 
    public ImmutableArray<AccountModel> Accounts { get; init; } = ImmutableArray<AccountModel>.Empty;

    [JsonPropertyName("pagingToken")]
    public string? PagingToken { get; init; } = null;
}