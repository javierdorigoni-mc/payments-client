using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PaymentsClient.Domain;
using PaymentsClient.Domain.Payments;
using PaymentsClient.WebApi.Controllers;

namespace PaymentsClient.WebApi.Tests;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class PaymentsControllerTests
{
    private Fixture _fixture;
    private PaymentsController _paymentsController;
    private Mock<IPaymentsService> _paymentsService;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _paymentsService = new Mock<IPaymentsService>();
        _paymentsController = new PaymentsController(_paymentsService.Object);
    }

    [Test]
    public async Task CreatePaymentAsync_WithPaymentsServiceReturningSuccess_Responds200OK()
    {
        // Arrange
        var createPaymentRequest = _fixture.Create<CreatePaymentRequest>();
        var createPaymentResponse = _fixture.Create<CreatePaymentResponse>();
        _paymentsService
            .Setup(x => x.CreatePaymentAsync(It.IsAny<CreatePaymentRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<CreatePaymentResponse>.Success(createPaymentResponse));

        // Act
        var result = (ObjectResult?)(await _paymentsController.CreatePaymentAsync(createPaymentRequest));

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.Not.Null);
        var payload = (CreatePaymentResponse)result.Value;
        Assert.That(payload, Is.EqualTo(createPaymentResponse));
        _paymentsService
            .Verify(
                x => x.CreatePaymentAsync(createPaymentRequest, It.IsAny<CancellationToken>()),
                Times.Once);
    }
    
    [Test]
    public async Task CreatePaymentAsync_WithPaymentsServiceReturningFailure_Responds400BadRequest()
    {      
        // Arrange
        var createPaymentRequest = _fixture.Create<CreatePaymentRequest>();
        _paymentsService
            .Setup(x => x.CreatePaymentAsync(It.IsAny<CreatePaymentRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<CreatePaymentResponse>.Failure("invalid request"));
        
        // Act
        var result = (ObjectResult?)(await _paymentsController.CreatePaymentAsync(createPaymentRequest));
    
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(400));      
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo("invalid request"));
        _paymentsService
            .Verify(
                x => x.CreatePaymentAsync(createPaymentRequest, It.IsAny<CancellationToken>()),
                Times.Once);
    }
    
    [Test]
    public async Task RefreshStatusAsync_WithPaymentsServiceReturningSuccess_Responds200OK()
    {
        // Arrange
        var refreshPaymentStatusRequest = _fixture.Create<RefreshPaymentStatusRequest>();
        var refreshPaymentStatusResponse = _fixture.Create<RefreshPaymentStatusResponse>();
        _paymentsService
            .Setup(x => x.RefreshStatusAsync(It.IsAny<RefreshPaymentStatusRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<RefreshPaymentStatusResponse>.Success(refreshPaymentStatusResponse));

        // Act
        var result = (ObjectResult?)(await _paymentsController.RefreshStatusAsync(refreshPaymentStatusRequest));

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.Not.Null);
        var payload = (RefreshPaymentStatusResponse)result.Value;
        Assert.That(payload, Is.EqualTo(refreshPaymentStatusResponse));
        _paymentsService
            .Verify(
                x => x.RefreshStatusAsync(refreshPaymentStatusRequest, It.IsAny<CancellationToken>()),
                Times.Once);
    }
    
    [Test]
    public async Task RefreshStatusAsync_WithPaymentsServiceReturningFailure_Responds400BadRequest()
    {      
        // Arrange
        var refreshPaymentStatusRequest = _fixture.Create<RefreshPaymentStatusRequest>();
        _paymentsService
            .Setup(x => x.RefreshStatusAsync(It.IsAny<RefreshPaymentStatusRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<RefreshPaymentStatusResponse>.Failure("invalid request"));
        
        // Act
        var result = (ObjectResult?)(await _paymentsController.RefreshStatusAsync(refreshPaymentStatusRequest));
    
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(400));      
        Assert.That(result.Value, Is.EqualTo("invalid request"));
        _paymentsService
            .Verify(
                x => x.RefreshStatusAsync(refreshPaymentStatusRequest, It.IsAny<CancellationToken>()),
                Times.Once);
    }
}