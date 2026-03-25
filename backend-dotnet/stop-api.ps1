# Stops a stray backend-dotnet host so `dotnet build` / `dotnet run` can replace bin output.
Get-Process -Name "backend-dotnet" -ErrorAction SilentlyContinue | Stop-Process -Force
Write-Host "backend-dotnet processes stopped (if any were running)."
