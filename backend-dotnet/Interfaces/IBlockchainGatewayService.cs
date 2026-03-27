using backend_dotnet.Models;

namespace backend_dotnet.Interfaces;

/// <summary>
/// Contract for fetching raw blockchain data from the Python AI service.
///
/// WHY is this called "Gateway" service?
///   In our architecture, .NET is the API Gateway — it orchestrates calls to
///   downstream services. This service is the bridge (gateway) between the
///   .NET layer and the Python AI microservice.
///
///   Naming matters: "BlockchainService" would imply it talks to the chain directly.
///   "BlockchainGatewayService" communicates that it proxies to another service.
///
/// WHY CancellationToken?
///   If the client disconnects or the request times out, ASP.NET Core signals
///   cancellation via the token. Every async I/O operation should respect this
///   so we don't waste resources on requests nobody is waiting for.
/// </summary>
public interface IBlockchainGatewayService
{
    /// <summary>
    /// Retrieves on-chain wallet data (balance, transaction count) for the
    /// given Ethereum address by delegating to the Python AI service.
    /// </summary>
    /// <param name="address">Checksummed EVM wallet address (0x + 40 hex chars).</param>
    /// <param name="cancellationToken">Propagates request cancellation.</param>
    Task<WalletAnalysisResponse> GetWalletDataAsync(
        string address,
        CancellationToken cancellationToken = default);
}
