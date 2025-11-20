# Build, test, migrate at runtime, and run the API in Render

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o código
COPY . .

# Restaura dependências
RUN dotnet restore NextStep.sln

# Executa testes (falha o build se quebrar)
RUN dotnet test NextStep.sln

# Publica a API
RUN dotnet publish NextStep.Api/NextStep.Api.csproj -c Release -o /app/publish

# Stage final (mantém SDK para rodar dotnet-ef em runtime)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app

# Copia artefatos publicados
COPY --from=build /app/publish .

# Script de entrada: aplica migrations e sobe a API
COPY entrypoint.sh .
RUN chmod +x /app/entrypoint.sh

# URLs para Render ($PORT é injetado pelo serviço)
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080}

ENTRYPOINT ["/app/entrypoint.sh"]
