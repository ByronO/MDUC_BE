namespace MDUC_BE.Api.Models;

public record CreateAccountRequest(
    string Name,
    long AccountTypeId,
    long AccountStatusId,
    string? ServiceableAddressId,
    string PrimaryContactName
    // TODO: Adjust fields according to Sonar schema
);

public record CreateAccountResponse(
    long Id,
    string Name
    // TODO: Include additional fields from Sonar response
);
