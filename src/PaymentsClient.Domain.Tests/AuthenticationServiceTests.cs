using AutoFixture;
using Moq;
using NUnit.Framework;
using PaymentsClient.Domain.Authentication;

namespace PaymentsClient.Domain.Tests;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class AuthenticationServiceTests
{        
    private Fixture _fixture;
    private Mock<INagApiClientService> _nagApiClientService;
    private AuthenticationService _authenticationService;

    [SetUp]
    public void Setup()
    {        
        _fixture = new Fixture();
        _nagApiClientService = new Mock<INagApiClientService>();
        _authenticationService = new AuthenticationService(_nagApiClientService.Object);
    }
    
    [Test]
    public async Task InitializeAuthenticationAsync_WithValidRequest_ReturnsExpectedResult()
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
        Assert.That(result, Is.EqualTo(expectedResponse));
        _nagApiClientService
            .Verify(x => x.InitializeAuthenticationAsync(request, It.IsAny<CancellationToken>())
                , Times.Once);
    }

    [Test]
    public async Task CompleteAuthenticationAsync_WithValidRequest_ReturnsExpectedResult()
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
        Assert.That(result, Is.EqualTo(expectedResponse));
        _nagApiClientService
            .Verify(x => x.ExchangeTokenAsync(request, It.IsAny<CancellationToken>())
                , Times.Once);
    }

    [Test]
    public async Task CompleteAuthenticationAsync_WithInvalidCode_ReturnsFailureResult()
    {
        // Arrange
        var request = _fixture.Build<CompleteAuthenticationRequest>()
                              .With(x => x.Code, string.Empty)
                              .Create();
        var expectedResponse = Result<CompleteAuthenticationResponse>.Failure("Invalid exchange token code");

        // Act
        var result = await _authenticationService.CompleteAuthenticationAsync(request);

        // Assert
        Assert.That(result.IsSuccessful, Is.False);
        Assert.That(result.Error, Is.EqualTo(expectedResponse.Error));
        _nagApiClientService
            .Verify(x => x.ExchangeTokenAsync(It.IsAny<CompleteAuthenticationRequest>(), It.IsAny<CancellationToken>())
                , Times.Never);
    }

    [Test]
    public async Task InitializeAuthenticationAsync_WithCancellationToken_CancellationTokenIsPassed()
    {
        // Arrange
        var request = _fixture.Create<InitializeAuthenticationRequest>();
        var expectedResponse = _fixture.Create<Result<InitializeAuthenticationResponse>>();
        var cancellationToken = new CancellationTokenSource().Token;
        _nagApiClientService
            .Setup(x => x.InitializeAuthenticationAsync(request, cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _authenticationService.InitializeAuthenticationAsync(request, cancellationToken);

        // Assert
        Assert.That(result, Is.EqualTo(expectedResponse));
        _nagApiClientService
            .Verify(x => x.InitializeAuthenticationAsync(request, cancellationToken)
                , Times.Once);
    }

    [Test]
    public async Task CompleteAuthenticationAsync_WithCancellationToken_CancellationTokenIsPassed()
    {
        // Arrange
        var request = _fixture.Create<CompleteAuthenticationRequest>();
        var expectedResponse = _fixture.Create<Result<CompleteAuthenticationResponse>>();
        var cancellationToken = new CancellationTokenSource().Token;
        _nagApiClientService
            .Setup(x => x.ExchangeTokenAsync(request, cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _authenticationService.CompleteAuthenticationAsync(request, cancellationToken);

        // Assert
        Assert.That(result, Is.EqualTo(expectedResponse));
        _nagApiClientService
            .Verify(x => x.ExchangeTokenAsync(request, cancellationToken)
                , Times.Once);
    }
}