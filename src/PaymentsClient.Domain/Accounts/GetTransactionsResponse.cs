using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace PaymentsClient.Domain.Accounts;

public record GetTransactionsResponse
{
    [JsonPropertyName("transactions")] 
    public ImmutableArray<TransactionModel> Transactions { get; init; } = ImmutableArray<TransactionModel>.Empty;

    [JsonPropertyName("pagingToken")]
    public string? PagingToken { get; init; } = null;
}