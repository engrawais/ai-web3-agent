using backend_dotnet.Models;

namespace backend_dotnet.Interfaces;

public interface IHealthService
{
    Task<HealthResponse> GetHealthAsync(CancellationToken cancellationToken = default);
}
