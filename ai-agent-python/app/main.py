"""FastAPI app factory: wiring only; logic lives in routes and services."""

from dotenv import load_dotenv
from fastapi import FastAPI

from app.routes.blockchain import router as blockchain_router
from app.routes.health import router as health_router

load_dotenv()


def create_app() -> FastAPI:
    app = FastAPI(
        title="AI-Powered Blockchain Analyst Agent API",
        version="0.1.0",
        docs_url="/docs",
        redoc_url="/redoc",
    )
    app.include_router(health_router)
    app.include_router(blockchain_router)
    return app


app = create_app()
