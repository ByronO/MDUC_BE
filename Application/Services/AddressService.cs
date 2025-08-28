using MDUC_BE.Api.Models;
using MDUC_BE.Infrastructure.GraphQL;

namespace MDUC_BE.Application.Services;

public class AddressService
{
    private readonly SonarGraphQLClient _client;
    public AddressService(SonarGraphQLClient client) => _client = client;

    public Task<IEnumerable<AddressDto>> GetByLine1Async(string line1, CancellationToken ct = default) => _client.GetAddressesByLine1Async(line1, ct);
}
