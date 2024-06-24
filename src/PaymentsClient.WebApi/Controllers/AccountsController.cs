using Microsoft.AspNetCore.Mvc;
using PaymentsClient.Domain.Accounts;

namespace PaymentsClient.WebApi.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController : ControllerBase
{    
    private readonly IAccountsService _accountsService;
    
    public AccountsController(IAccountsService accountsService)
    {
        _accountsService = accountsService;
    }

    [HttpPost]
    public async Task<IActionResult> GetAccountsAsync(
        [FromBody] GetAccountsRequest request, 
        CancellationToken cancellationToken = default)
    {
        var result = await _accountsService.GetAccountsAsync(request, cancellationToken);

        if (result.IsSuccessful)
        {
            return Ok(result.Value);
        }
        
        return BadRequest(result.Error);
    }
    
    [HttpPost("transactions")]
    public async Task<IActionResult> GetTransactionsAsync(
        [FromBody] GetTransactionsRequest request, 
        CancellationToken cancellationToken = default)
    {
        var result = await _accountsService.GetTransactionsAsync(request, cancellationToken);
    
        if (result.IsSuccessful)
        {
            return Ok(result.Value);
        }
        
        return BadRequest(result.Error);
    }
}