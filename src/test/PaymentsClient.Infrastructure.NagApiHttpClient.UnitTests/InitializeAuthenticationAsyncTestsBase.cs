using System.Net;
using System.Net.Http.Json;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using PaymentsClient.Domain.Authentication;

namespace PaymentsClient.Infrastructure.NagApiHttpClient.UnitTests;

public class InitializeAuthenticationAsyncTestsBase : NagApiHttpClientServiceTestsBase
{
    [Test]
    [InlineAutoData("https://auth.example.com/auth/start", "some-session-id")]
    public async Task GivenHttpMessageHandlerReturns200OK_ThenResultIsSuccessful(
        string expectedAuthUrl,
        string expectedSessionId,
        string userHash, 
        Uri redirectUrl)
    {
        // Arrange
        var request = new InitializeAuthenticationRequest(UserHash: userHash, RedirectUrl: redirectUrl.AbsoluteUri);
        
        HttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(new InitializeAuthenticationResponse()
                {
                    AuthUrl = expectedAuthUrl,
                    SessionId = expectedSessionId
                })
            });

        // Act
        var result = await NagApiHttpClientService.InitializeAuthenticationAsync(request);

        // Assert
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(string.Equals(result.Value.AuthUrl, expectedAuthUrl), Is.True);
        Assert.That(string.Equals(result.Value.SessionId, expectedSessionId), Is.True);
        HttpMessageHandler
            .Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == new Uri("https://api.test.com/v1/authentication/initialize") &&
                    req.Headers.Contains("X-Client-Id") &&
                    req.Headers.Contains("X-Client-Secret") && 
                    req.Content != null),
                ItExpr.IsAny<CancellationToken>());
    }
    
    [Test, AutoData]
    public async Task GivenHttpMessageHandlerFails_ThenResultContainsErrors(string userHash, Uri redirectUrl)
    {
        // Arrange
        var request = new InitializeAuthenticationRequest(UserHash: userHash, RedirectUrl: redirectUrl.AbsoluteUri);
        
        HttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Request failed"));

        // Act
        var result = await NagApiHttpClientService.InitializeAuthenticationAsync(request);

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