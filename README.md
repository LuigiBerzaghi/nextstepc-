## 📖 Sobre o Projeto

**NextStep** é uma plataforma inovadora que utiliza **IA (Google Gemini)** para democratizar a requalificação profissional. O sistema analisa currículos, identifica lacunas de conhecimento e gera **jornadas personalizadas** com recursos curados de plataformas como Coursera, Udemy, YouTube e também graduações, caso necessário.

### 📊 Contexto e Dados de Mercado

De acordo com a **ONU**, **OIT** e **Fórum Econômico Mundial**:

- 📈 **170 milhões de empregos** serão criados entre 2025-2030
- 🔄 **23% das profissões** vão se transformar radicalmente até 2027
- 🤖 **40% das tarefas humanas** podem ser automatizadas nos próximos 5 anos
- ⚡ **60% em 10 anos** - automação em escala acelerada
- 🎓 **Milhões de profissionais** precisarão se requalificar até 2030

**O desafio:** *Como equilibrar eficiência tecnológica com o valor humano?*  
**Nossa resposta:** *IA personaliza, mas VOCÊ decide seu caminho.*

### 🎯 Objetivos de Desenvolvimento Sustentável (ODS)

NextStep contribui diretamente com 4 ODS destacados pela Global Solution:

| ODS | Descrição | Como NextStep Contribui |
|-----|-----------|-------------------------|
| **🎓 ODS 4** | Educação de Qualidade | Acesso democratizado a trilhas de aprendizado personalizadas |
| **💼 ODS 8** | Trabalho Decente e Crescimento Econômico | Requalificação profissional para empregos dignos |
| **🏭 ODS 9** | Indústria, Inovação e Infraestrutura | Uso de IA e tecnologias emergentes |
| **⚖️ ODS 10** | Redução das Desigualdades | Plataforma gratuita, inclusiva e acessível |

---

### 🔥 Por que NextStep?

**O trabalho está mudando. E você pode ajudar a criar o que vem pela frente.**

> *Não fique para trás. **Dê o próximo passo.***  
> Sua próxima carreira começa hoje, com uma jornada personalizada criada por IA.

**NextStep** não é apenas uma plataforma — é o seu parceiro na maior transformação profissional da história. Enquanto o mundo se prepara para 170 milhões de novos empregos e a extinção de milhares de outros, a pergunta não é **se** você vai se requalificar, mas **quando** e **como**.

**A resposta? Agora. Com NextStep.**

#### 🎯 Problema que Resolve
- 🌀 **Profissionais perdidos** em transições de carreira
- 🔍 **Dificuldade em identificar gaps** de conhecimento
- 📚 **Sobrecarga de informação** - qual curso fazer?
- 🛤️ **Falta de trilhas personalizadas** e estruturadas
- ⏰ **Urgência de requalificação** em um mercado em transformação
- 💸 **Barreiras financeiras** para cursos de qualidade

#### 💡 Nossa Solução
- 🤖 **Análise de currículo com IA** (Google Gemini)
- 🎯 **Jornadas personalizadas** baseadas no perfil e objetivo profissional
- 📚 **Curadoria inteligente** de recursos externos gratuitos e pagos
- 💬 **Chatbot assistente** para dúvidas e motivação
- 📊 **Dashboard visual** de evolução e progresso
- 🌐 **Modelo agregador** - conectamos você ao melhor conteúdo do mercado

#### 🌟 Diferencial: Tecnologia + Lado Humano
- ✅ IA analisa e recomenda, mas **você mantém o controle** da sua jornada
- ✅ Foco em **habilidades humanas**: criatividade, empatia, pensamento crítico
- ✅ **Aprender e reaprender**: o novo superpoder da era digital

---

## 🏗️Arquitetura

| Projeto | Responsabilidade |
| --- | --- |
| `NextStep.Domain` | Entidades, enums e regras centrais (ex.: cálculo de progresso de jornada). |
| `NextStep.Application` | DTOs, exceções, serviços de aplicação, contratos e geração de JWT. |
| `NextStep.Infrastructure` | EF Core + Oracle (ODP.NET), repositórios, `NextStepDbContext`, migrations e seeds. |
| `NextStep.Api` | Controllers versionados, autenticação JWT, HATEOAS, health check, observabilidade. |
| `NextStep.Tests` | Testes unitários (domínio) e de integração (WebApplicationFactory). |


## 🎯Testes

Os testes são executador pelo Dockerfile antes de realizar o deploy na nuvem através do comando:

```
dotnet test NextStep.sln
```

- **Unitários** — validação de regras de jornada (progresso e status).
- **Integração** — fluxo completo com `WebApplicationFactory`, cobrindo registro e jornada ativa via HTTP real.



Health check disponível em `GET /health`. 

Todos os endpoints (exceto `/auth`) exigem `Authorization: Bearer <token>`.

## 💾Banco de dados
Feito utilizando **Oracle** e com as variáveis de ambiente cadastradas no Render.

## 🌐Como acessar

Abra o link : https://nextstepc.onrender.com/swagger/index.html

## 📄Swagger & Demonstração

- Após iniciar a API.
- A UI exibe **todas as versões publicadas (v1 e v2)** usando API Versioning; basta escolher no seletor e testar.
- O botão **Authorize** habilita enviar o JWT obtido em `/api/v1/auth/login` ou `/api/v1/auth/register`.
- Cada endpoint traz descrição/resumo, códigos de resposta e modelos de payload para demonstração.

## 💻Versionamento da API

- Configurado com **URL Segment** via `Microsoft.AspNetCore.Mvc.Versioning`.
- Endpoints expostos em `/api/v1/...` e `/api/v2/...` (ambos apontando para a mesma implementação atual, prontos para evoluções breaking).
- Exemplos: `/api/v1/auth/login`, `/api/v2/journeys/active`, `/api/v1/professions?pageNumber=1&pageSize=10`.

## 💻Endpoints Principais

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

## 👀Observabilidade

- Logging com `ILogger` (middlewares e controllers) + scoping por `X-Correlation-Id`.
- `CorrelationIdMiddleware` lê/gera `X-Correlation-Id`, adiciona aos logs/respostas.
- `RequestTracingMiddleware` cria `Activity` via `ActivitySource("NextStep.Api")` e adiciona tags (método, rota, correlationId).
- `GET /health` expõe health check com verificação do `NextStepDbContext`.

## 👨‍⚕️Health (GET)

Caso o app e DB estejam OK, retonará:
```json
Healthy
```

caso contrário:

```json 
Unhealthy

```

---

## 👥 Equipe

- RM555516 - Luigi Berzaghi  
- RM559093 - Guilherme Pelissari   
- RM558445 - Cauã dos Santos 
