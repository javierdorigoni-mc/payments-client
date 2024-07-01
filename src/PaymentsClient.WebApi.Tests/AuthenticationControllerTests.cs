using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PaymentsClient.Domain;
using PaymentsClient.WebApi.Controllers;
using PaymentsClient.Domain.Authentication;

namespace PaymentsClient.WebApi.Tests;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class AuthenticationControllerTests
{
    private AuthenticationController _authenticationController;
    private Mock<IAuthenticationService> _authenticationService;

    [SetUp]
    public void Setup()
    {
        _authenticationService = new Mock<IAuthenticationService>();
        _authenticationController = new AuthenticationController(_authenticationService.Object);
    }
    
    [Test, AutoData]
    public async Task InitializeAsync_WithAuthenticationServiceReturningSuccess_Responds200OK(string userHash, Uri redirectUrl, string authUrl, string sessionId)
    {
        // Arrange
        _authenticationService
            .Setup(x => x.InitializeAuthenticationAsync(It.IsAny<InitializeAuthenticationRequest>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<InitializeAuthenticationResponse>.Success(new InitializeAuthenticationResponse() {AuthUrl = authUrl, SessionId = sessionId}));
        var request = new InitializeAuthenticationRequest(UserHash: userHash, RedirectUrl: redirectUrl.AbsoluteUri);
        
        // Act
        var result = (ObjectResult?)(await _authenticationController.InitializeAsync(request));

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));      
        Assert.That(result.Value, Is.Not.Null);
        var payload = (InitializeAuthenticationResponse)result.Value;
        Assert.That(payload.AuthUrl, Is.EqualTo(authUrl));
        Assert.That(payload.SessionId, Is.EqualTo(sessionId));
        _authenticationService
            .Verify(
                x => x.InitializeAuthenticationAsync(request, It.IsAny<CancellationToken>()),
                Times.Once);
    }
    
    [Test, AutoData]
    public async Task InitializeAsync_WithAuthenticationServiceReturningFailure_Responds400BadRequest(string userHash, Uri redirectUrl)
    {
        // Arrange
        _authenticationService
            .Setup(x => x.InitializeAuthenticationAsync(It.IsAny<InitializeAuthenticationRequest>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<InitializeAuthenticationResponse>.Failure("invalid request"));
        var request = new InitializeAuthenticationRequest(UserHash: userHash, RedirectUrl: redirectUrl.AbsoluteUri);
        
        // Act
        var result = (ObjectResult?)(await _authenticationController.InitializeAsync(request));

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(400));      
        Assert.That(result.Value, Is.EqualTo("invalid request"));

        _authenticationService
            .Verify(
                x => x.InitializeAuthenticationAsync(request, It.IsAny<CancellationToken>()),
                Times.Once);
    }
    
    [Test, AutoData]
    public async Task CompleteAsync_WithAuthenticationServiceReturningSuccess_Responds200OK(string code)
    {
        // Arrange
        _authenticationService
            .Setup(x => x.CompleteAuthenticationAsync(It.IsAny<CompleteAuthenticationRequest>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<CompleteAuthenticationResponse>.Success(new CompleteAuthenticationResponse()));
        var request = new CompleteAuthenticationRequest(code);
        
        // Act
        var result = (ObjectResult?)(await _authenticationController.CompleteAsync(request));

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));      
        Assert.That(result.Value, Is.Not.Null);
        var payload = (CompleteAuthenticationResponse)result.Value;
        Assert.That(payload, Is.Not.Null);
        _authenticationService
            .Verify(
                x => x.CompleteAuthenticationAsync(request, It.IsAny<CancellationToken>()),
                Times.Once);
    }
    
    [Test, AutoData]
    public async Task CompleteAsync_WithAuthenticationServiceReturningFailure_Responds400BadRequest(string code)
    {
        // Arrange
        _authenticationService
            .Setup(x => x.CompleteAuthenticationAsync(It.IsAny<CompleteAuthenticationRequest>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<CompleteAuthenticationResponse>.Failure("invalid request"));
        var request = new CompleteAuthenticationRequest(code);
        
        // Act
        var result = (ObjectResult?)(await _authenticationController.CompleteAsync(request));

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(400));      
        Assert.That(result.Value, Is.EqualTo("invalid request"));
        _authenticationService
            .Verify(
                x => x.CompleteAuthenticationAsync(request, It.IsAny<CancellationToken>()),
                Times.Once);
    }
}