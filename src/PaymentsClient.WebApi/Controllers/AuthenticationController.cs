using Microsoft.AspNetCore.Mvc;
using PaymentsClient.Domain.Authentication;

namespace PaymentsClient.WebApi.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{    
    private readonly IAuthenticationService _authenticationService;
    
    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("initialize")]
    public async Task<IActionResult> InitializeAsync(
        [FromBody] InitializeAuthenticationRequest request, 
        CancellationToken cancellationToken = default)
    {
        var result = await _authenticationService.InitializeAuthenticationAsync(request, cancellationToken);

        if (result.IsSuccessful)
        {
            return Ok(result.Value);
        }
        
        return BadRequest(result.Error);
    }
    
    [HttpPost("complete")]
    public async Task<IActionResult> CompleteAsync(
        [FromBody] CompleteAuthenticationRequest request, 
        CancellationToken cancellationToken = default)
    {
        var result = await _authenticationService.CompleteAuthenticationAsync(request, cancellationToken);

        if (result.IsSuccessful)
        {
            return Ok(result.Value);
        }
        
        return BadRequest(result.Error);
    }
}