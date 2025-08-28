using MDUC_BE.Api.Filters;
using MDUC_BE.Application.Services;

namespace MDUC_BE.Api.Endpoints;

public static class AddressEndpoints
{
    public static RouteGroupBuilder MapAddressEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/addresses", async (string line1, AddressService service) =>
        {
            var addresses = await service.GetByLine1Async(line1);
            return Results.Ok(addresses);
        })
        .AddEndpointFilter<ValidateAddressQueryFilter>()
        .WithName("GetAddresses")
        .WithSummary("Get serviceable addresses by line1")
        .WithOpenApi();

        return group;
    }
}
