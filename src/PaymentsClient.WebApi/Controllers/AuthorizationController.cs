using Microsoft.AspNetCore.Mvc;
using PaymentsClient.Domain.Models;
using PaymentsClient.Domain.Services;

namespace PaymentsClient.WebApi.Controllers;

[ApiController]
[Route("api/authorization")]
public class AuthorizationController : ControllerBase
{    
    private readonly INagApiClientService _nagApiClientService;
    private readonly ILogger<AuthorizationController> _logger;
    
    public AuthorizationController(
        INagApiClientService nagApiClientService,
        ILogger<AuthorizationController> logger)
    {
        _nagApiClientService = nagApiClientService;
        _logger = logger;
    }

    [HttpPost("initialize")]
    public async Task<IActionResult> InitializeAsync(
        [FromBody] InitializeAuthorizationRequest initializeAuthorizationRequest, 
        CancellationToken cancellationToken = default)
    {
        var result = await _nagApiClientService.InitializeAuthenticationAsync(initializeAuthorizationRequest, cancellationToken);

        if (result.IsSuccessful)
        {
            return Ok(result.Value);
        }
        
        return BadRequest(result.Error);
    }
}