using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NextStep.Application.DTOs.Auth;
using NextStep.Application.DTOs.Chat;
using NextStep.Application.DTOs.Journeys;
using NextStep.Application.DTOs.Profile;
using NextStep.Application.DTOs.Resume;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NextStep.Api.Infrastructure.Swagger;

public class ExampleSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema is null)
        {
            return;
        }

        var demoEmail = "aline.souza@nextstep.com";
        var demoPassword = "SenhaForte123!";
        var demoConversation = "aline-mentor-journey";

        schema.Example = context.Type switch
        {
            Type t when t == typeof(LoginRequest) => OpenApiAnyFactory.CreateFromJson($$"""
            {
              "email": "{{demoEmail}}",
              "password": "{{demoPassword}}"
            }
            """),
            Type t when t == typeof(RegisterRequest) => OpenApiAnyFactory.CreateFromJson($$"""
            {
              "email": "{{demoEmail}}",
              "password": "{{demoPassword}}",
              "name": "Aline Souza",
              "currentJob": "Analista de Dados"
            }
            """),
            Type t when t == typeof(UpdateProfileRequest) => OpenApiAnyFactory.CreateFromJson("""
            {
              "name": "Aline Souza",
              "currentJob": "Tech Lead - IA",
              "newEmail": "aline.souza@nextstep.com",
              "currentPassword": "SenhaForte123!",
              "newPassword": "NovaSenha!456"
            }
            """),
            Type t when t == typeof(CreateJourneyRequest) => OpenApiAnyFactory.CreateFromJson("""
            {
              "desiredJob": "AI Product Strategist",
              "currentSkills": [
                "C#",
                "Azure",
                "Data Storytelling"
              ],
              "gaps": [
                "IA Generativa aplicada",
                "Governança de dados"
              ]
            }
            """),
            Type t when t == typeof(UpdateJourneyStepProgressRequest) => OpenApiAnyFactory.CreateFromJson("""
            {
              "progress": 85
            }
            """),
            Type t when t == typeof(ResumeUploadRequest) => OpenApiAnyFactory.CreateFromJson("""
            {
              "resumeText": "Especialista em dados com 6 anos em analytics, projetos em Azure Synapse, Python e IA para operações. Busco migrar para estratégia em AI Products."
            }
            """),
            Type t when t == typeof(ChatSendRequest) => OpenApiAnyFactory.CreateFromJson($$"""
            {
              "conversationId": "{{demoConversation}}",
              "message": "Quais passos devo seguir para migrar de dados para estratégia de IA?"
            }
            """),
            _ => schema.Example
        };
    }
}
