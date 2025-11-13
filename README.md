# NextStep – Future of Work API

NextStep é uma plataforma de requalificação profissional. Esta versão em **ASP.NET Core** fornece autenticação simplificada, gestão de perfil, jornadas de aprendizado inteligentes, análise de currículo, dashboard, chat com mentor de IA e catálogo de profissões alinhados ao tema **“O Futuro do Trabalho”**.

## Arquitetura

| Projeto | Responsabilidade |
| --- | --- |
| `NextStep.Domain` | Entidades, enums e regras centrais (ex.: cálculo de progresso de jornada). |
| `NextStep.Application` | DTOs, exceções, serviços de aplicação, contratos e geração de JWT. |
| `NextStep.Infrastructure` | EF Core + SQL Server, repositórios, `NextStepDbContext`, migrations e seeds. |
| `NextStep.Api` | Controllers versionados, autenticação JWT, HATEOAS, health check, observabilidade. |
| `NextStep.Tests` | Testes unitários (domínio) e de integração (WebApplicationFactory). |

## Como iniciar

Clone o repositório no diretório desejado:

```powershell
git clone <repo>
```

Navegue até o diretório do projeto:

```powershell
cd <dir>
```

## Configuração

1. **Connection string** : 
  Atualize o as credenciais em `appsettings.json` (`ConnectionStrings:DefaultConnection`).           
  O arquivo contém a observação para alterar **servidor**, **Usuário** e **senha** (caso necessário) antes de usar:  
    `"DefaultConnection": "Server=localhost;Database=NextStepDb;User Id=sa;Password=SenhaForte123!;TrustServerCertificate=True;"`

2. **JWT** — troque `Jwt:SecretKey` por um valor forte em produção.

## Banco & Migrations

```powershell
# Restaurar dependências
dotnet restore NextStep.sln
```

```powershell
# Aplicar migrations no SQL Server configurado
dotnet ef database update --project NextStep.Infrastructure --startup-project NextStep.Api
```

## Testes

```powershell
dotnet test NextStep.sln
```

- **Unitários** — validação de regras de jornada (progresso e status).
- **Integração** — fluxo completo com `WebApplicationFactory`, cobrindo registro e jornada ativa via HTTP real.


## Execução

```powershell
# Subir a API
dotnet run --project NextStep.Api
```

Health check disponível em `GET /health`. Todos os endpoints (exceto `/auth`) exigem `Authorization: Bearer <token>`.

## Swagger & Demonstração

- Após iniciar a API, acesse `https://localhost:<porta>/swagger`.
- A UI exibe **todas as versões publicadas (v1 e v2)** usando API Versioning; basta escolher no seletor e testar.
- O botão **Authorize** habilita enviar o JWT obtido em `/api/v1/auth/login` ou `/api/v1/auth/register`.
- Cada endpoint traz descrição/resumo, códigos de resposta e modelos para demonstração.

## Versionamento da API

- Configurado com **URL Segment** via `Microsoft.AspNetCore.Mvc.Versioning`.
- Endpoints expostos em `/api/v1/...` e `/api/v2/...` (ambos apontando para a mesma implementação atual, prontos para evoluções breaking).
- Exemplos: `/api/v1/auth/login`, `/api/v2/journeys/active`, `/api/v1/professions?pageNumber=1&pageSize=10`.

## Endpoints Principais

- `POST /api/v1/auth/register` – cria usuário e retorna JWT.
- `POST /api/v1/auth/login` – autenticação simplificada.
- `GET /api/v1/profile` – perfil do usuário autenticado.
- `PUT /api/v1/profile` – atualiza nome/cargo e, opcionalmente, e-mail e senha (para alterar senha/e-mail envie `currentPassword` + `newPassword` e/ou `newEmail`).
- `DELETE /api/v1/profile` – remove o usuário definitivamente (cascade em jornadas, chat, etc.).
- `POST /api/v1/journeys` – gera jornada inteligente com passos automáticos.
- `GET /api/v1/journeys/active` – jornada ativa com HATEOAS (`self`, `updateStep`, `history`).
- `PATCH /api/v1/journeys/steps/{stepId}/progress` – atualiza progresso de um passo.
- `GET /api/v1/journeys/history` – histórico paginado com metadados + links HATEOAS.
- `POST /api/v1/resume/upload` – envia o currículo (JSON) para gerar insights fake.
- `GET /api/v1/resume/analysis/latest` – retorna o último resultado estruturado (skills/gaps/carreiras).
- `GET /api/v1/dashboard` – resumo com próximo passo, métricas e médias.
- `POST /api/v1/chat/send` / `GET /api/v1/chat/history` – mentor IA simulado e histórico paginado.
- `GET /api/v1/professions` – catálogo de profissões com busca (`search`) e paginação.

## Observabilidade

- Logging com `ILogger` (middlewares e controllers) + scoping por `X-Correlation-Id`.
- `CorrelationIdMiddleware` lê/gera `X-Correlation-Id`, adiciona aos logs/respostas.
- `RequestTracingMiddleware` cria `Activity` via `ActivitySource("NextStep.Api")` e adiciona tags (método, rota, correlationId).
- `GET /health` expõe health check com verificação do `NextStepDbContext`.

## Health (GET)

Caso o app e DB estejam OK, retonará:
```json
Healthy
```

caso contrário:

```json 
Unhealthy

```

---

Para parar a aplicação, basta pressionar:

```
cntrl + c
```

## 👥 Equipe

- RM555516 - Luigi Berzaghi  
- RM559093 - Guilherme Pelissari   
- RM558445 - Cauã dos Santos 