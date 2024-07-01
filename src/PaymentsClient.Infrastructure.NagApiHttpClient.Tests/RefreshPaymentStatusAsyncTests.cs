using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using PaymentsClient.Domain.Payments;

namespace PaymentsClient.Infrastructure.NagApiHttpClient.Tests;

public class RefreshPaymentStatusAsyncTests : NagApiHttpClientServiceTestsBase
{
    [Test, AutoData]
    public async Task RefreshPaymentStatusAsync_HttpMessageHandlerReturns200OK_ResultIsSuccessful(string paymentId)
    {
        // Arrange
        var request = new RefreshPaymentStatusRequest(paymentId);
        var paymentDetailsResponse = FixtureBuilder.Build<PaymentDetailsModel>()
            .With(r => r.PaymentId, paymentId)
            .Create();
        var response = FixtureBuilder.Build<RefreshPaymentStatusResponse>()
            .With(r => r.PaymentDetails, paymentDetailsResponse)
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
        var result = await NagApiHttpClientService.RefreshPaymentStatusAsync(request);

        // Assert
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.PaymentDetails, Is.Not.Null);
        Assert.That(result.Value.PaymentDetails.PaymentId, Is.EqualTo(paymentId));
        HttpMessageHandler
            .Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == new Uri($"https://api.test.com/v1/payments/{paymentId}/refresh-status") &&
                    req.Headers.Contains("X-Client-Id") &&
                    req.Headers.Contains("X-Client-Secret")),
                ItExpr.IsAny<CancellationToken>());
    }
    
    [Test]
    public async Task RefreshPaymentStatusAsync_HttpMessageHandlerFails_ResultContainsErrors()
    {
        // Arrange
        var request = FixtureBuilder.Create<RefreshPaymentStatusRequest>();
        
        HttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Request failed"));

        // Act
        var result = await NagApiHttpClientService.RefreshPaymentStatusAsync(request);

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