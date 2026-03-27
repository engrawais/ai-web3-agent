"""On-chain reads via web3.py (Sepolia by default)."""

from __future__ import annotations

import os
import re

from web3 import Web3

_ADDRESS_RE = re.compile(r"^0x[a-fA-F0-9]{40}$")


def _rpc_url() -> str:
    return os.getenv("ETH_RPC_URL", "https://rpc.sepolia.org").strip()


def fetch_wallet_snapshot(address: str) -> tuple[str, float, int]:
    """
    Returns (checksummed address, balance in ETH, transaction count / nonce).

    Raises:
        ValueError: malformed address
        RuntimeError: RPC unreachable or call failed
    """
    if not _ADDRESS_RE.match(address):
        raise ValueError("Must be a valid Ethereum address (0x + 40 hex characters).")

    w3 = Web3(Web3.HTTPProvider(_rpc_url(), request_kwargs={"timeout": 30}))
    if not w3.is_connected():
        raise RuntimeError("Cannot connect to Ethereum RPC. Check ETH_RPC_URL.")

    checksum = Web3.to_checksum_address(address)
    balance_wei = w3.eth.get_balance(checksum)
    tx_count = w3.eth.get_transaction_count(checksum)
    balance_eth = float(Web3.from_wei(balance_wei, "ether"))
    return checksum, balance_eth, int(tx_count)
