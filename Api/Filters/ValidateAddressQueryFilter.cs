using MDUC_BE.Common.Errors;
using MDUC_BE.Common.Models;

namespace MDUC_BE.Api.Filters;

public class ValidateAddressQueryFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var line1 = context.HttpContext.Request.Query["line1"].ToString();
        if (string.IsNullOrWhiteSpace(line1) || line1.Length < 3)
        {
            var error = new ErrorResponse(context.HttpContext.TraceIdentifier, ApiErrorCode.Validation, "line1 must be at least 3 characters", null);
            return Results.Json(error, statusCode: StatusCodes.Status400BadRequest);
        }

        return await next(context);
    }
}
