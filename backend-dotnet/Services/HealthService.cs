using backend_dotnet.Interfaces;
using backend_dotnet.Models;

namespace backend_dotnet.Services;

public sealed class HealthService : IHealthService
{
    private static readonly HealthResponse Ok = new("ok");

    public Task<HealthResponse> GetHealthAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(Ok);
    }
}
