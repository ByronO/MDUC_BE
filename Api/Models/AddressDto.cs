namespace MDUC_BE.Api.Models;

public record AddressDto(
    string Id,
    string Line1,
    string? Line2,
    string City,
    string? Subdivision,
    string Zip,
    string Country,
    double? Latitude,
    double? Longitude,
    bool Serviceable,
    string AddressableType);
