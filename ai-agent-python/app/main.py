"""FastAPI app factory: wiring only; logic lives in routes and services."""

from fastapi import FastAPI

from app.routes.health import router as health_router


def create_app() -> FastAPI:
    app = FastAPI(
        title="AI-Powered Blockchain Analyst Agent API",
        version="0.1.0",
        docs_url="/docs",
        redoc_url="/redoc",
    )
    app.include_router(health_router)
    return app


app = create_app()
