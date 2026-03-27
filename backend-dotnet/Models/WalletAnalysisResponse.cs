namespace backend_dotnet.Models;

/// <summary>
/// Response DTO returned to the API consumer AND used internally to deserialize
/// the Python AI service's JSON response.
///
/// WHY one record for both purposes? The Python service and this API share
/// the same data shape for wallet info. No need to create a separate internal model
/// until the shapes diverge (e.g., when we add AI scores in Phase 3).
///
/// JSON CONTRACT ALIGNMENT:
///   .NET serializes this as camelCase by default (ASP.NET Core web defaults):
///     Address        → "address"
///     BalanceEth     → "balanceEth"
///     TransactionCount → "transactionCount"
///
///   Python's Pydantic model uses alias_generator=to_camel to produce the same shape.
///   This is the agreed internal contract between both services.
///
/// WHY decimal for BalanceEth (not double/float)?
///   ETH balances need precision. `decimal` avoids floating-point rounding errors
///   that `double` or `float` would introduce. Always use decimal for money/currency values.
///
/// WHY long for TransactionCount?
///   The transaction count is a wallet's nonce — it grows with every transaction.
///   Highly active wallets (exchanges, protocols) can have millions of transactions.
///   `int` (max ~2 billion) is safe for now, but `long` is future-proof at zero cost.
/// </summary>
public sealed record WalletAnalysisResponse(
    string Address,
    decimal BalanceEth,
    long TransactionCount
);
