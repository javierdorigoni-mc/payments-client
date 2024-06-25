using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentsClient.WebApi.Controllers;

namespace PaymentsClient.WebApi.UnitTests;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class PingControllerTests
{
    private readonly PingController _pingController;
    private readonly Mock<ILogger<PingController>> _logger;

    public PingControllerTests()
    {
        _logger = new Mock<ILogger<PingController>>();
        _pingController = new PingController(_logger.Object);
    }
    
    [Test]
    public void WhenPingCalled_ThenPongResponded()
    {
        // Arrange
        
        // Act
        var result = (ObjectResult?)(_pingController.Ping());

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));      
        Assert.That(result.Value, Is.Not.Null); 
        Assert.That(string.Equals(result.Value.ToString(), "Pong"));
        _logger
            .Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
    }
}