using Microsoft.AspNetCore.Mvc;
using PaymentsClient.Domain.Models;

namespace PaymentsClient.WebApi.Controllers;

[ApiController]
[Route("api/authorization")]
public class AuthorizationController : ControllerBase
{
    private readonly ILogger<PingController> _logger;

    public AuthorizationController(ILogger<PingController> logger)
    {
        _logger = logger;
    }

    [HttpPost("authorization/initialize")]
    public IActionResult InitializeAuthorization(
        [FromBody] InitializeAuthorizationRequest initializeAuthorizationRequest, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Ping called");
        return Ok("Pong");
    }
}