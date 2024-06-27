using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using PaymentsClient.Domain.Payments;

namespace PaymentsClient.Infrastructure.NagApiHttpClient.UnitTests;

public class CreatePaymentAsyncTests : NagApiHttpClientServiceTestsBase
{
    [Test]
    public async Task GivenHttpMessageHandlerReturns200OK_ThenResultIsSuccessful()
    {
        // Arrange
        var request = FixtureBuilder.Create<CreatePaymentRequest>();
        var response = FixtureBuilder.Create<CreatePaymentResponse>();
        
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
        var result = await NagApiHttpClientService.CreatePaymentAsync(request);

        // Assert
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(string.IsNullOrWhiteSpace(result.Value.PaymentId), Is.False);
        Assert.That(string.IsNullOrWhiteSpace(result.Value.RedirectUrl), Is.False);
        HttpMessageHandler
            .Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == new Uri("https://api.test.com/v1/payments/create") &&
                    req.Headers.Contains("X-Client-Id") &&
                    req.Headers.Contains("X-Client-Secret")),
                ItExpr.IsAny<CancellationToken>());
    }
    
    [Test]
    public async Task GivenHttpMessageHandlerFails_ThenResultContainsErrors()
    {
        // Arrange
        var request = FixtureBuilder.Create<CreatePaymentRequest>();
        
        HttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Request failed"));

        // Act
        var result = await NagApiHttpClientService.CreatePaymentAsync(request);

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