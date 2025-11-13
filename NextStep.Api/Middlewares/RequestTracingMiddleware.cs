using System.Diagnostics;

namespace NextStep.Api.Middlewares;

public class RequestTracingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ActivitySource _activitySource;
    private readonly ILogger<RequestTracingMiddleware> _logger;

    public RequestTracingMiddleware(RequestDelegate next, ActivitySource activitySource, ILogger<RequestTracingMiddleware> logger)
    {
        _next = next;
        _activitySource = activitySource;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using var activity = _activitySource.StartActivity($"{context.Request.Method} {context.Request.Path}");
        if (activity is not null)
        {
            if (context.Items.TryGetValue("CorrelationId", out var correlationId))
            {
                activity.SetTag("correlationId", correlationId);
            }

            activity.SetTag("http.method", context.Request.Method);
            activity.SetTag("http.url", context.Request.Path);
        }

        _logger.LogInformation("Handling {Method} {Path}", context.Request.Method, context.Request.Path);
        await _next(context);
    }
}
