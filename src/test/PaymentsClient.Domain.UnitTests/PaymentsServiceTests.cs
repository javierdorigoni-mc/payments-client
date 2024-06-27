using AutoFixture;
using Moq;
using PaymentsClient.Domain.Accounts;
using PaymentsClient.Domain.Payments;

namespace PaymentsClient.Domain.UnitTests;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class PaymentsServiceTests
{        
    private readonly Fixture _fixture;
    private readonly Mock<INagApiClientService> _nagApiClientService;
    private readonly PaymentsService _paymentsService;

    public PaymentsServiceTests()
    {
        _fixture = new Fixture();
        _nagApiClientService = new Mock<INagApiClientService>();
        _paymentsService = new PaymentsService(_nagApiClientService.Object);
    }
    
    [Test]
    public async Task GivenValidRequest_WhenCreatePaymentAsyncCalled_ThenReturnsExpectedResult()
    {
        // Arrange
        var request = _fixture.Build<CreatePaymentRequest>()
                              .With(r => r.RedirectUrl, "https://valid.url")
                              .Create();
        var expectedResponse = _fixture.Create<Result<CreatePaymentResponse>>();
        _nagApiClientService
            .Setup(x => x.CreatePaymentAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _paymentsService.CreatePaymentAsync(request);

        // Assert
        Assert.That(result, Is.EqualTo(expectedResponse));
        _nagApiClientService
            .Verify(x => x.CreatePaymentAsync(request, It.IsAny<CancellationToken>())
                , Times.Once);
    }

    [Test]
    public async Task GivenInvalidRedirectUrl_WhenCreatePaymentAsyncCalled_ThenReturnsFailureResult()
    {
        // Arrange
        var request = _fixture.Build<CreatePaymentRequest>()
                              .With(r => r.RedirectUrl, "invalid-url")
                              .Create();
        var expectedResponse = Result<CreatePaymentResponse>.Failure("Invalid redirectUrl");

        // Act
        var result = await _paymentsService.CreatePaymentAsync(request);

        // Assert
        Assert.That(result.Error, Is.EqualTo(expectedResponse.Error));
        Assert.That(result.IsSuccessful, Is.False);
        _nagApiClientService
            .Verify(x => x.CreatePaymentAsync(It.IsAny<CreatePaymentRequest>(), It.IsAny<CancellationToken>())
                , Times.Never);
    }

    [Test]
    public async Task GivenInvalidAmount_WhenCreatePaymentAsyncCalled_ThenReturnsFailureResult()
    {
        // Arrange
        var invalidCreatePaymentRequestDetails = _fixture.Build<CreatePaymentRequestDetails>()
            .With(r => r.Amount, new AmountModel()
            {
                Currency = null, 
                Value = null
            })
            .Create();
        var request = _fixture.Build<CreatePaymentRequest>()
                              .With(r => r.RedirectUrl, "https://valid.url")
                              .With(r => r.Request, invalidCreatePaymentRequestDetails)
                              .Create();
        var expectedResponse = Result<CreatePaymentResponse>.Failure("Invalid destination amount");

        // Act
        var result = await _paymentsService.CreatePaymentAsync(request);

        // Assert
        Assert.That(result.Error, Is.EqualTo(expectedResponse.Error));
        Assert.That(result.IsSuccessful, Is.False);
        _nagApiClientService
            .Verify(x => x.CreatePaymentAsync(It.IsAny<CreatePaymentRequest>(), It.IsAny<CancellationToken>())
                , Times.Never);    }

    [Test]
    public async Task GivenValidRequest_WhenRefreshStatusAsyncCalled_ThenReturnsExpectedResult()
    {
        // Arrange
        var request = _fixture.Create<RefreshPaymentStatusRequest>();
        var expectedResponse = _fixture.Create<Result<RefreshPaymentStatusResponse>>();
        _nagApiClientService
            .Setup(x => x.RefreshPaymentStatusAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _paymentsService.RefreshStatusAsync(request);

        // Assert
        Assert.That(result, Is.EqualTo(expectedResponse));
        _nagApiClientService
            .Verify(x => x.RefreshPaymentStatusAsync(request, It.IsAny<CancellationToken>())
                , Times.Once);
    }

    [Test]
    public async Task GivenInvalidPaymentId_WhenRefreshStatusAsyncCalled_ThenReturnsFailureResult()
    {
        // Arrange
        var request = _fixture.Build<RefreshPaymentStatusRequest>()
                              .With(r => r.PaymentId, string.Empty)
                              .Create();
        var expectedResponse = Result<RefreshPaymentStatusResponse>.Failure("Invalid paymentId");

        // Act
        var result = await _paymentsService.RefreshStatusAsync(request);

        // Assert
        Assert.That(result.Error, Is.EqualTo(expectedResponse.Error));
        Assert.That(result.IsSuccessful, Is.False);
        _nagApiClientService
            .Verify(x => x.RefreshPaymentStatusAsync(It.IsAny<RefreshPaymentStatusRequest>(), It.IsAny<CancellationToken>())
                , Times.Never);
    }

    [Test]
    public async Task GivenCancellationToken_WhenCreatePaymentAsyncCalled_ThenCancellationTokenIsPassed()
    {
        // Arrange
        var request = _fixture.Build<CreatePaymentRequest>()
                              .With(r => r.RedirectUrl, "https://valid.url")
                              .Create();
        var expectedResponse = _fixture.Create<Result<CreatePaymentResponse>>();
        var cancellationToken = new CancellationTokenSource().Token;
        _nagApiClientService
            .Setup(x => x.CreatePaymentAsync(request, cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _paymentsService.CreatePaymentAsync(request, cancellationToken);

        // Assert
        Assert.That(result, Is.EqualTo(expectedResponse));
        _nagApiClientService
            .Verify(x => x.CreatePaymentAsync(request, cancellationToken)
                , Times.Once);
    }

    [Test]
    public async Task GivenCancellationToken_WhenRefreshStatusAsyncCalled_ThenCancellationTokenIsPassed()
    {
        // Arrange
        var request = _fixture.Create<RefreshPaymentStatusRequest>();
        var expectedResponse = _fixture.Create<Result<RefreshPaymentStatusResponse>>();
        var cancellationToken = new CancellationTokenSource().Token;
        _nagApiClientService
            .Setup(x => x.RefreshPaymentStatusAsync(request, cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _paymentsService.RefreshStatusAsync(request, cancellationToken);

        // Assert
        Assert.That(result, Is.EqualTo(expectedResponse));
        _nagApiClientService
            .Verify(x => x.RefreshPaymentStatusAsync(request, cancellationToken)
                , Times.Once);
    }
}