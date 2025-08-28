using System.Net.Http.Headers;
using System.Net.Http.Json;
using MDUC_BE.Api.Models;

namespace MDUC_BE.Infrastructure.GraphQL;

public class SonarGraphQLClient
{
    private readonly HttpClient _client;
    private readonly ILogger<SonarGraphQLClient> _logger;

    public SonarGraphQLClient(HttpClient client, IConfiguration configuration, ILogger<SonarGraphQLClient> logger)
    {
        _client = client;
        _logger = logger;

        var baseUrl = configuration["Sonar:GraphQLUrl"];
        if (!string.IsNullOrEmpty(baseUrl))
        {
            _client.BaseAddress = new Uri(baseUrl);
        }

        var apiKey = configuration["Sonar:ApiKey"];
        if (!string.IsNullOrEmpty(apiKey))
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }
    }

    public async Task<T?> SendAsync<T>(string query, object? variables = null, string? operationName = null, CancellationToken ct = default)
    {
        var request = new GraphQLRequest
        {
            Query = query,
            Variables = variables,
            OperationName = operationName
        };

        using var response = await _client.PostAsJsonAsync(string.Empty, request, ct);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Sonar returned status {StatusCode}", response.StatusCode);
            throw new HttpRequestException($"Sonar error {(int)response.StatusCode}");
        }

        var gql = await response.Content.ReadFromJsonAsync<GraphQLResponse<T>>(cancellationToken: ct);
        if (gql?.Errors?.Any() == true)
        {
            var messages = string.Join(";", gql.Errors.Select(e => e.Message));
            _logger.LogError("GraphQL errors: {Errors}", messages);
            throw new Exception(messages);
        }

        return gql?.Data;
    }

    public virtual async Task<IEnumerable<AddressDto>> GetAddressesByLine1Async(string line1, CancellationToken ct = default)
    {
        const string query = @"query ServiceableAddresses($line1: String!) {
  addresses(serviceable:true, type:PHYSICAL, general_search:$line1) {
    entities {
      id
      line1
      line2
      city
      subdivision
      zip
      country
      latitude
      longitude
      serviceable
      addressable_type
    }
  }
}";

        var result = await SendAsync<AddressSearchData>(query, new { line1 }, "ServiceableAddresses", ct);
        return result?.Addresses.Entities ?? Enumerable.Empty<AddressDto>();
    }

    public virtual async Task<CreateAccountResponse?> CreateAccountAsync(CreateAccountRequest input, CancellationToken ct = default)
    {
        const string mutation = @"mutation CreateAccount($input: CreateAccountInput!) {
  createAccount(input:$input) {
    id
    name
  }
}";

        var variables = new
        {
            input = new
            {
                name = input.Name,
                account_type_id = input.AccountTypeId,
                account_status_id = input.AccountStatusId,
                serviceable_address_id = input.ServiceableAddressId,
                primary_contact = new { name = input.PrimaryContactName }
                // TODO: Adjust to Sonar schema
            }
        };

        var result = await SendAsync<CreateAccountPayload>(mutation, variables, "CreateAccount", ct);
        return result?.CreateAccount;
    }

    private record AddressSearchData(AddressCollection Addresses);
    private record AddressCollection(IEnumerable<AddressDto> Entities);
    private record CreateAccountPayload(CreateAccountResponse CreateAccount);
}
