using MDUC_BE.Api.Models;
using MDUC_BE.Infrastructure.GraphQL;

namespace MDUC_BE.Application.Services;

public class AccountService
{
    private readonly SonarGraphQLClient _client;
    public AccountService(SonarGraphQLClient client) => _client = client;

    public Task<CreateAccountResponse?> CreateAsync(CreateAccountRequest request, CancellationToken ct = default) => _client.CreateAccountAsync(request, ct);
}
