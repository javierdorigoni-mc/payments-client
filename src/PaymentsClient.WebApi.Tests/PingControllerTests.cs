using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PaymentsClient.WebApi.Controllers;

namespace PaymentsClient.WebApi.Tests;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class PingControllerTests
{
    private PingController _pingController;
    private Mock<ILogger<PingController>> _logger;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<PingController>>();
        _pingController = new PingController(_logger.Object);
    }
    
    [Test]
    public void PingCalled_Responds200OkWithPong()
    {
        // Arrange
        
        // Act
        var result = (ObjectResult?)(_pingController.Ping());

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));      
        Assert.That(result.Value, Is.EqualTo("Pong"));
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