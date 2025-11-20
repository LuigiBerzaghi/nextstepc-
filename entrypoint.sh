#!/bin/sh
set -e

# Garante que dotnet-ef está acessível no PATH
export PATH="$PATH:/root/.dotnet/tools"

# Aplica migrations no Oracle (usa ConnectionStrings__DefaultConnection das variáveis de ambiente)
dotnet ef database update --no-build --project NextStep.Infrastructure --startup-project NextStep.Api

# Sobe a API
exec dotnet NextStep.Api.dll --urls "${ASPNETCORE_URLS:-http://0.0.0.0:8080}"
