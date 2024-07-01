using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using PaymentsClient.Domain.Accounts;

namespace PaymentsClient.Infrastructure.NagApiHttpClient.Tests;

public class GetAccountsAsyncTests : NagApiHttpClientServiceTestsBase
{
    [Test]
    public async Task GetAccountsAsync_HttpMessageHandlerReturns200OK_ResultIsSuccessful()
    {
        // Arrange
        var request = FixtureBuilder.Create<GetAccountsRequest>();
        var singleAccountResponse = FixtureBuilder.Create<AccountModel>();
        var response = FixtureBuilder
            .Build<GetAccountsResponse>()
            .With(r => r.Accounts, [singleAccountResponse])
            .Create();

        HttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(response)
            });

        // Act
        var result = await NagApiHttpClientService.GetAccountsAsync(request);

        // Assert
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.Accounts, Is.Not.Empty);
        HttpMessageHandler
            .Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("https://api.test.com/v2/accounts") &&
                    req.Headers.Contains("X-Client-Id") &&
                    req.Headers.Contains("X-Client-Secret") && 
                    req.Headers.Contains("Authorization")),
                ItExpr.IsAny<CancellationToken>());
    }
    
    [Test]
    public async Task GetAccountsAsync_HttpMessageHandlerFails_ResultContainsErrors()
    {
        // Arrange
        var request = FixtureBuilder.Create<GetAccountsRequest>();        
        
        HttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Request failed"));

        // Act
        var result = await NagApiHttpClientService.GetAccountsAsync(request);

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
}