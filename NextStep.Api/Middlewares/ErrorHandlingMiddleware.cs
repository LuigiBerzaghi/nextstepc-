using System.Net;
using System.Text.Json;
using NextStep.Application.Exceptions;

namespace NextStep.Api.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AppException ex)
        {
            await HandleAsync(context, ex.StatusCode, MapErrorCode(ex.StatusCode), ex.Message, ex.Details, ex);
        }
        catch (Exception ex)
        {
            var correlationId = context.Items.TryGetValue("CorrelationId", out var correlation)
                ? correlation?.ToString()
                : null;

            _logger.LogError(ex, "Unhandled exception. CorrelationId: {CorrelationId}", correlationId);

            var payload = new
            {
                error = "INTERNAL_SERVER_ERROR",
                message = "Erro inesperado",
                correlationId
            };

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }

    private async Task HandleAsync(HttpContext context, HttpStatusCode statusCode, string errorCode, string message, object? details, Exception exception)
    {
        _logger.LogWarning(exception, "Handled application exception: {Message}", message);

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            error = errorCode,
            message,
            details
        }));
    }

    private static string MapErrorCode(HttpStatusCode statusCode) =>
        statusCode switch
        {
            HttpStatusCode.NotFound => "NOT_FOUND",
            HttpStatusCode.Unauthorized => "UNAUTHORIZED",
            HttpStatusCode.Forbidden => "FORBIDDEN",
            HttpStatusCode.UnprocessableEntity => "UNPROCESSABLE_ENTITY",
            _ => "BAD_REQUEST"
        };
}
