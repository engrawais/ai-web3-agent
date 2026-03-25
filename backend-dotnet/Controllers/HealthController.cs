using backend_dotnet.Interfaces;
using backend_dotnet.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet.Controllers;

[ApiController]
[Route("api")]
public sealed class HealthController(IHealthService healthService) : ControllerBase
{
    [HttpGet("health")]
    [ProducesResponseType(typeof(HealthResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<HealthResponse>> GetAsync(CancellationToken cancellationToken)
    {
        var payload = await healthService.GetHealthAsync(cancellationToken);
        return Ok(payload);
    }
}
