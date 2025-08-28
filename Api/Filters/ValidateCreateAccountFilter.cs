using MDUC_BE.Api.Models;
using MDUC_BE.Common.Errors;
using MDUC_BE.Common.Models;

namespace MDUC_BE.Api.Filters;

public class ValidateCreateAccountFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.Arguments.FirstOrDefault(a => a is CreateAccountRequest) as CreateAccountRequest;
        if (request is null || string.IsNullOrWhiteSpace(request.Name))
        {
            var error = new ErrorResponse(context.HttpContext.TraceIdentifier, ApiErrorCode.Validation, "Name is required", null);
            return Results.Json(error, statusCode: StatusCodes.Status400BadRequest);
        }

        return await next(context);
    }
}
