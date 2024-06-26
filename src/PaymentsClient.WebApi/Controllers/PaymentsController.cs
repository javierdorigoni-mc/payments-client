using Microsoft.AspNetCore.Mvc;
using PaymentsClient.Domain.Payments;

namespace PaymentsClient.WebApi.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{    
    private readonly IPaymentsService _paymentsService;
    
    public PaymentsController(IPaymentsService paymentsService)
    {
        _paymentsService = paymentsService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePaymentAsync(
        [FromBody] CreatePaymentRequest request, 
        CancellationToken cancellationToken = default)
    {
        var result = await _paymentsService.CreatePaymentAsync(request, cancellationToken);

        if (result.IsSuccessful)
        {
            return Ok(result.Value);
        }
        
        return BadRequest(result.Error);
    }
    
    [HttpPost("status")]
    public async Task<IActionResult> RefreshStatusAsync(
        [FromBody] RefreshPaymentStatusRequest request, 
        CancellationToken cancellationToken = default)
    {
        var result = await _paymentsService.RefreshStatusAsync(request, cancellationToken);

        if (result.IsSuccessful)
        {
            return Ok(result.Value);
        }
        
        return BadRequest(result.Error);
    }
}