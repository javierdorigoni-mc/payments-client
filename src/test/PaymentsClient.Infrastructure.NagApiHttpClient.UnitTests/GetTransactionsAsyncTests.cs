using System.Collections.Immutable;
using System.Net;
using System.Net.Http.Json;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using PaymentsClient.Domain.Accounts;

namespace PaymentsClient.Infrastructure.NagApiHttpClient.UnitTests;

public class GetTransactionsAsyncTests : NagApiHttpClientServiceTestsBase
{
    [Test]
    [AutoData]
    public async Task GivenHttpMessageHandlerReturns200OK_ThenResultIsSuccessful(
        string accessToken,
        string? pagingToken,
        string accountId,
        bool withDetails)
    {
        // Arrange
        var fromDate = "2021-01-01";
        var request = new GetTransactionsRequest(
            AccessToken: accessToken,
            AccountId: accountId,
            FromDate: fromDate,
            WithDetails: withDetails);
        
        HttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(new GetTransactionsResponse()
                {
                    Transactions = ImmutableArray<TransactionModel>.Empty,
                    PagingToken = pagingToken
                })
            });

        // Act
        var result = await NagApiHttpClientService.GetTransactionsAsync(request);

        // Assert
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.Transactions, Is.Empty);
        Assert.That(string.Equals(result.Value.PagingToken, pagingToken), Is.True);
        HttpMessageHandler
            .Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri($"https://api.test.com/v2/accounts/{accountId}/transactions?fromDate={fromDate}&withDetails={withDetails.ToString().ToLowerInvariant()}") &&
                    req.Headers.Contains("X-Client-Id") &&
                    req.Headers.Contains("X-Client-Secret") && 
                    req.Headers.Contains("Authorization")),
                ItExpr.IsAny<CancellationToken>());
    }
    
    [Test, AutoData]
    public async Task GivenHttpMessageHandlerFails_ThenResultContainsErrors(string accessToken, string accountId)
    {
        // Arrange
        var request = new GetTransactionsRequest(AccessToken: accessToken, AccountId: accountId);
        
        HttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Request failed"));

        // Act
        var result = await NagApiHttpClientService.GetTransactionsAsync(request);

        // Assert
        Assert.That(result.IsSuccessful, Is.False);
        Assert.That(string.Equals(result.Error, "There is an issue with your request, please verify the logs."), Is.True);
        Logger
            .Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
    }
}