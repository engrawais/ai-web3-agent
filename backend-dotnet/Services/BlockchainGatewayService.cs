using System.Net.Http.Json;
using backend_dotnet.Interfaces;
using backend_dotnet.Models;

namespace backend_dotnet.Services;

/// <summary>
/// Calls the Python AI service to fetch raw on-chain wallet data.
///
/// ARCHITECTURE PATTERN — Typed HttpClient:
///   This class receives a pre-configured HttpClient via constructor injection.
///   The BaseAddress, timeout, and headers are set once in DependencyInjection.cs.
///   Here we only write the "what to call" — not the "how to connect".
///   Separation of concerns: configuration is infrastructure, calling is service logic.
///
/// WHY sealed?
///   We never intend to subclass this. `sealed` is a small performance hint to the
///   JIT compiler and communicates clear design intent.
///
/// REAL-WORLD ANALOGY:
///   Think of this service as your office's "external affairs department".
///   It knows how to talk to the Python service (the external partner).
///   The controller doesn't need to know how that communication works.
/// </summary>
public sealed class BlockchainGatewayService(HttpClient httpClient) : IBlockchainGatewayService
{
    /// <summary>
    /// Makes a GET request to the Python service: GET /blockchain/wallet/{address}
    ///
    /// WHY ReadFromJsonAsync?
    ///   It's a one-liner that reads the response stream and deserializes JSON.
    ///   It does NOT buffer the full response into a string first — more memory efficient.
    ///   It also respects the CancellationToken throughout the I/O operation.
    ///
    /// WHY throw HttpRequestException (not catch it)?
    ///   This service's job is to call Python and return data.
    ///   It does NOT know what HTTP status code to return to the client — that's
    ///   the controller's responsibility (HTTP is the controller's domain).
    ///   We let the exception bubble up and the controller maps it to a 502.
    /// </summary>
    public async Task<WalletAnalysisResponse> GetWalletDataAsync(
        string address,
        CancellationToken cancellationToken = default)
    {
        // EnsureSuccessStatusCode throws HttpRequestException for 4xx/5xx responses.
        // ReadFromJsonAsync returns null if the body is empty or "null" literal.
        // The null-forgiving operator (!) is safe here because EnsureSuccessStatusCode
        // already guarantees a valid 2xx body from the Python service.
        var response = await httpClient.GetAsync(
            $"/blockchain/wallet/{address}",
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var data = await response.Content
            .ReadFromJsonAsync<WalletAnalysisResponse>(cancellationToken: cancellationToken);

        return data!;
    }
}
