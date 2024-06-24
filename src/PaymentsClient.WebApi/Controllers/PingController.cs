using Microsoft.AspNetCore.Mvc;

namespace PaymentsClient.WebApi.Controllers;

[ApiController]
[Route("api")]
public class PingController : ControllerBase
{
    private readonly ILogger<PingController> _logger;

    public PingController(ILogger<PingController> logger)
    {
        _logger = logger;
    }

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        _logger.LogInformation("Ping called");
        return Ok("Pong");
    }
}