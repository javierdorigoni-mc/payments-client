using System.Collections.Immutable;
using System.Net;
using System.Net.Http.Json;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using PaymentsClient.Domain.Accounts;

namespace PaymentsClient.Infrastructure.NagApiHttpClient.Tests;

public class GetTransactionsAsyncTests : NagApiHttpClientServiceTestsBase
{
    [Test]
    [AutoData]
    public async Task GetTransactionsAsync_HttpMessageHandlerReturns200OK_ResultIsSuccessful(
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
        Assert.That(result.Value.PagingToken, Is.EqualTo(pagingToken));
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
    public async Task GetTransactionsAsync_HttpMessageHandlerFails_ResultContainsErrors(string accessToken, string accountId)
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
        Assert.That(result.Error, Is.EqualTo("There is an issue with your request, please verify the logs."));
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
    
    [Test, AutoData]
    public async Task GetTransactionsAsync_ForbiddenResponse_ResultContainsForbiddenError(string accessToken, string accountId)
    {
        // Arrange
        var request = new GetTransactionsRequest(AccessToken: accessToken, AccountId: accountId);
        
        HttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("some forbidden message", new UnauthorizedAccessException("expired access token"), HttpStatusCode.Forbidden));

        // Act
        var result = await NagApiHttpClientService.GetTransactionsAsync(request);

        // Assert
        Assert.That(result.IsSuccessful, Is.False);
        Assert.That(result.Error, Is.EqualTo("Forbidden"));
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