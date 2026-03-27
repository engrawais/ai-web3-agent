from pydantic import BaseModel, ConfigDict, Field


class WalletAnalysisResponse(BaseModel):
    """Matches gateway DTO: camelCase JSON for inter-service contract."""

    model_config = ConfigDict(populate_by_name=True)

    address: str
    balance_eth: float = Field(..., serialization_alias="balanceEth")
    transaction_count: int = Field(..., serialization_alias="transactionCount")
