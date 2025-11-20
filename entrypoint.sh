#!/bin/sh
set -e

# Garante que dotnet-ef está acessível no PATH
export PATH="$PATH:/root/.dotnet/tools"

# Usa o diretório com os csproj
cd /app

# Ajusta a URL de escuta (Render injeta PORT)
PORT="${PORT:-8080}"
export ASPNETCORE_URLS="http://0.0.0.0:${PORT}"

# Aplica migrations no Oracle (usa ConnectionStrings__DefaultConnection das variáveis de ambiente)
if [ "${RUN_MIGRATIONS:-true}" = "true" ]; then
  set +e
  if command -v timeout >/dev/null 2>&1; then
    timeout "${MIGRATION_TIMEOUT:-90}s" dotnet ef database update --project NextStep.Infrastructure/NextStep.Infrastructure.csproj --startup-project NextStep.Api/NextStep.Api.csproj
  else
    dotnet ef database update --project NextStep.Infrastructure/NextStep.Infrastructure.csproj --startup-project NextStep.Api/NextStep.Api.csproj
  fi
  EXIT_CODE=$?
  set -e
  if [ $EXIT_CODE -ne 0 ]; then
    echo "WARNING: migrations failed (exit code $EXIT_CODE); starting API anyway." >&2
  fi
fi

# Sobe a API já publicada
exec dotnet /app/publish/NextStep.Api.dll --urls "${ASPNETCORE_URLS:-http://0.0.0.0:8080}"
