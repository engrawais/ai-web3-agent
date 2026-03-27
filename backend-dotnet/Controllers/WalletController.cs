using System.Net.Http;
using backend_dotnet.Interfaces;
using backend_dotnet.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet.Controllers;

[ApiController]
[Route("api/wallet")]
public sealed class WalletController(IBlockchainGatewayService blockchainGateway) : ControllerBase
{
    /// <summary>Delegates to the Python service for on-chain balance and nonce (tx count).</summary>
    [HttpPost("analyze")]
    [ProducesResponseType(typeof(WalletAnalysisResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
    public async Task<ActionResult<WalletAnalysisResponse>> AnalyzeAsync(
        [FromBody] WalletRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await blockchainGateway.GetWalletDataAsync(request.Address, cancellationToken);
            return Ok(result);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(
                StatusCodes.Status502BadGateway,
                new { message = "Python AI service unreachable or returned an error.", detail = ex.Message });
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return StatusCode(StatusCodes.Status504GatewayTimeout, new { message = "Python AI service timed out." });
        }
    }
}
