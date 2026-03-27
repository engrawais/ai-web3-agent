from fastapi import APIRouter, HTTPException

from app.models.wallet import WalletAnalysisResponse
from app.services.blockchain_service import fetch_wallet_snapshot

router = APIRouter(prefix="/blockchain", tags=["blockchain"])


@router.get("/wallet/{address}", response_model=WalletAnalysisResponse)
async def get_wallet(address: str) -> WalletAnalysisResponse:
    try:
        checksum, balance_eth, tx_count = fetch_wallet_snapshot(address)
        return WalletAnalysisResponse(
            address=checksum,
            balance_eth=balance_eth,
            transaction_count=tx_count,
        )
    except ValueError as e:
        raise HTTPException(status_code=400, detail=str(e)) from e
    except RuntimeError as e:
        raise HTTPException(status_code=502, detail=str(e)) from e
