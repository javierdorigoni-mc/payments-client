using AutoFixture;
using Moq;
using PaymentsClient.Domain.Authentication;

namespace PaymentsClient.Domain.UnitTests;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class AuthenticationServiceTests
{        
    private readonly Fixture _fixture;
    private readonly Mock<INagApiClientService> _nagApiClientService;
    private readonly AuthenticationService _authenticationService;

    public AuthenticationServiceTests()
    {        
        _fixture = new Fixture();
        _nagApiClientService = new Mock<INagApiClientService>();
        _authenticationService = new AuthenticationService(_nagApiClientService.Object);
    }
    
    [Test]
    public async Task GivenValidRequest_WhenInitializeAuthenticationAsyncCalled_ThenReturnsExpectedResult()
    {
        // Arrange
        var request = _fixture.Create<InitializeAuthenticationRequest>();
        var expectedResponse = _fixture.Create<Result<InitializeAuthenticationResponse>>();
        _nagApiClientService
            .Setup(x => x.InitializeAuthenticationAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _authenticationService.InitializeAuthenticationAsync(request);

        // Assert
        Assert.That(expectedResponse.Equals(result), Is.True);
        _nagApiClientService
            .Verify(x => x.InitializeAuthenticationAsync(request, It.IsAny<CancellationToken>())
                , Times.Once);
    }

    [Test]
    public async Task GivenValidRequest_WhenCompleteAuthenticationAsyncCalled_ThenReturnsExpectedResult()
    {
        // Arrange
        var request = _fixture.Create<CompleteAuthenticationRequest>();
        var expectedResponse = _fixture.Create<Result<CompleteAuthenticationResponse>>();
        _nagApiClientService
            .Setup(x => x.ExchangeTokenAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _authenticationService.CompleteAuthenticationAsync(request);

        // Assert
        Assert.That(expectedResponse.Equals(result), Is.True);
        _nagApiClientService.Verify(x => x.ExchangeTokenAsync(request, It.IsAny<CancellationToken>())
            , Times.Once);
    }

    [Test]
    public async Task GivenInvalidCode_WhenCompleteAuthenticationAsyncCalled_ThenReturnsFailureResult()
    {
        // Arrange
        var request = _fixture.Build<CompleteAuthenticationRequest>()
                              .With(x => x.Code, string.Empty)
                              .Create();
        var expectedResponse = Result<CompleteAuthenticationResponse>.Failure("Invalid exchange token code");

        // Act
        var result = await _authenticationService.CompleteAuthenticationAsync(request);

        // Assert
        Assert.That(expectedResponse.Error.Equals(result.Error), Is.True);
        Assert.That(result.IsSuccessful, Is.False);
        _nagApiClientService
            .Verify(x => x.ExchangeTokenAsync(It.IsAny<CompleteAuthenticationRequest>(), It.IsAny<CancellationToken>())
                , Times.Never);
    }

    [Test]
    public async Task GivenCancellationToken_WhenInitializeAuthenticationAsyncCalled_ThenCancellationTokenIsPassed()
    {
        // Arrange
        var request = _fixture.Create<InitializeAuthenticationRequest>();
        var expectedResponse = _fixture.Create<Result<InitializeAuthenticationResponse>>();
        var cancellationToken = new CancellationTokenSource().Token;
        _nagApiClientService.Setup(x => x.InitializeAuthenticationAsync(request, cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _authenticationService.InitializeAuthenticationAsync(request, cancellationToken);

        // Assert
        Assert.That(expectedResponse.Equals(result), Is.True);
        _nagApiClientService.Verify(x => x.InitializeAuthenticationAsync(request, cancellationToken), Times.Once);
    }

    [Test]
    public async Task GivenCancellationToken_WhenCompleteAuthenticationAsyncCalled_ThenCancellationTokenIsPassed()
    {
        // Arrange
        var request = _fixture.Create<CompleteAuthenticationRequest>();
        var expectedResponse = _fixture.Create<Result<CompleteAuthenticationResponse>>();
        var cancellationToken = new CancellationTokenSource().Token;
        _nagApiClientService.Setup(x => x.ExchangeTokenAsync(request, cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _authenticationService.CompleteAuthenticationAsync(request, cancellationToken);

        // Assert
        Assert.That(expectedResponse.Equals(result), Is.True);
        _nagApiClientService.Verify(x => x.ExchangeTokenAsync(request, cancellationToken), Times.Once);
    }
}