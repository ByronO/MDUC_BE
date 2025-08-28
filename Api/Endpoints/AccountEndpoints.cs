using MDUC_BE.Api.Filters;
using MDUC_BE.Api.Models;
using MDUC_BE.Application.Services;

namespace MDUC_BE.Api.Endpoints;

public static class AccountEndpoints
{
    public static RouteGroupBuilder MapAccountEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/account", async (CreateAccountRequest request, AccountService service) =>
        {
            var result = await service.CreateAsync(request);
            return Results.Ok(result);
        })
        .AddEndpointFilter<ValidateCreateAccountFilter>()
        .WithName("CreateAccount")
        .WithSummary("Create account")
        .WithOpenApi();

        return group;
    }
}
