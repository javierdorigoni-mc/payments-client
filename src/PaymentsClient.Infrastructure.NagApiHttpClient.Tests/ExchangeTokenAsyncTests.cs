using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using PaymentsClient.Domain.Authentication;

namespace PaymentsClient.Infrastructure.NagApiHttpClient.Tests;

public class ExchangeTokenAsyncTests : NagApiHttpClientServiceTestsBase
{
    [Test]
    public async Task ExchangeTokenAsync_HttpMessageHandlerReturns200OK_ResultIsSuccessful()
    {
        // Arrange
        var request = FixtureBuilder.Create<CompleteAuthenticationRequest>(); 
        var response = FixtureBuilder.Create<CompleteAuthenticationResponse>();

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
        var result = await NagApiHttpClientService.ExchangeTokenAsync(request);

        // Assert
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.Login, Is.Not.Null);
        Assert.That(result.Value.Session, Is.Not.Null);
        Assert.That(result.Value.ProviderId, Is.Not.Null);
        HttpMessageHandler
            .Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == new Uri("https://api.test.com/v1/authentication/tokens") &&
                    req.Headers.Contains("X-Client-Id") &&
                    req.Headers.Contains("X-Client-Secret") && 
                    req.Content != null),
                ItExpr.IsAny<CancellationToken>());
    }
    
    [Test]
    public async Task ExchangeTokenAsync_HttpMessageHandlerFails_ResultContainsErrors()
    {
        // Arrange
        var request = FixtureBuilder.Create<CompleteAuthenticationRequest>(); 
        
        HttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Request failed"));

        // Act
        var result = await NagApiHttpClientService.ExchangeTokenAsync(request);

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