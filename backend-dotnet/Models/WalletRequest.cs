using System.ComponentModel.DataAnnotations;

namespace backend_dotnet.Models;

/// <summary>
/// Incoming request body for wallet analysis.
///
/// WHY a record? Records are immutable value types — perfect for DTOs.
/// Once the request is created, no one should mutate it mid-pipeline.
///
/// WHY validation attributes? [ApiController] auto-validates the model
/// before the controller action even runs. If the address is bad, the client
/// gets a 400 Bad Request with clear field-level error messages — zero controller code needed.
/// </summary>
public sealed record WalletRequest
{
    /// <summary>
    /// A valid EVM wallet address: "0x" prefix followed by exactly 40 hex characters.
    ///
    /// WHY validate here and not in the service? The gateway's job is to enforce
    /// the API contract. Malformed addresses should never reach the Python service
    /// or the blockchain. Fail fast, fail early.
    /// </summary>
    [Required(ErrorMessage = "Wallet address is required.")]
    [RegularExpression(
        @"^0x[a-fA-F0-9]{40}$",
        ErrorMessage = "Must be a valid Ethereum address (0x followed by 40 hex characters).")]
    public required string Address { get; init; }
}
