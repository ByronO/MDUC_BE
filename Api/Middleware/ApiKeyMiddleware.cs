using MDUC_BE.Common.Errors;
using MDUC_BE.Common.Models;

namespace MDUC_BE.Api.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiKeyMiddleware> _logger;
    private readonly string? _expectedKey;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<ApiKeyMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _expectedKey = configuration["PublicApiKey"];
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!string.IsNullOrEmpty(_expectedKey) && context.Request.Headers.TryGetValue("X-API-Key", out var provided) && provided == _expectedKey)
        {
            await _next(context);
            return;
        }

        _logger.LogWarning("Invalid API key");
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsJsonAsync(new ErrorResponse(context.TraceIdentifier, ApiErrorCode.Unauthorized, "Invalid API key", null));
    }
}

public static class ApiKeyMiddlewareExtensions
{
    public static IApplicationBuilder UseApiKey(this IApplicationBuilder app) => app.UseMiddleware<ApiKeyMiddleware>();
}
