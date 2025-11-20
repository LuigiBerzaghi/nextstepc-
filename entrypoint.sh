#!/bin/sh
set -e

# Garante que dotnet-ef está acessível no PATH
export PATH="$PATH:/root/.dotnet/tools"

# Usa o diretório com os csproj
cd /app

# Aplica migrations no Oracle (usa ConnectionStrings__DefaultConnection das variáveis de ambiente)
dotnet ef database update --project NextStep.Infrastructure/NextStep.Infrastructure.csproj --startup-project NextStep.Api/NextStep.Api.csproj

# Sobe a API já publicada
exec dotnet /app/publish/NextStep.Api.dll --urls "${ASPNETCORE_URLS:-http://0.0.0.0:8080}"
