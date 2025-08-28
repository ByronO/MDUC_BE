# Sonar GraphQL Proxy

Minimal API using .NET 9 that proxies requests to the Sonar GraphQL API. It exposes a small surface area ready to expand.

## Configuration

Values can be stored in `appsettings.json`, environment variables or user-secrets:

- `Sonar:GraphQLUrl` – e.g. `https://<host>/graphql`.
- `Sonar:ApiKey` – server-side Sonar API key.
- `PublicApiKey` – value expected in `X-API-Key` header.
- `Cors:AllowedOrigins` – array of origins allowed by CORS.

## Running

```bash
dotnet run
```

## Testing

```bash
dotnet test
```

## Endpoints

- `GET /api/addresses?line1={text}` – search serviceable addresses.
- `POST /api/account` – create a new account.

## Notes

GraphQL query and mutation fields are placeholders. Adjust the schema sections in `SonarGraphQLClient` to match the actual Sonar API.
