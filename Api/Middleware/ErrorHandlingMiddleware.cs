using MDUC_BE.Common.Errors;
using MDUC_BE.Common.Models;

namespace MDUC_BE.Api.Middleware;

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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var error = new ErrorResponse(context.TraceIdentifier, ApiErrorCode.Unexpected, "Unexpected error", null);
            await context.Response.WriteAsJsonAsync(error);
        }
    }
}

public static class ErrorHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalErrorHandler(this IApplicationBuilder app) => app.UseMiddleware<ErrorHandlingMiddleware>();
}
