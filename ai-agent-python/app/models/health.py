from pydantic import BaseModel, ConfigDict, Field


class HealthResponse(BaseModel):
    """JSON contract for liveness (matches gateway shape)."""

    model_config = ConfigDict(extra="forbid")

    status: str = Field(examples=["ok"])
