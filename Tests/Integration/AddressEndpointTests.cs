using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MDUC_BE.Api.Models;
using MDUC_BE.Infrastructure.GraphQL;
using MDUC_BE;

namespace MDUC_BE.Tests.Integration;

public class AddressEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AddressEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var mock = new Mock<SonarGraphQLClient>(new HttpClient(), Mock.Of<IConfiguration>(), Mock.Of<ILogger<SonarGraphQLClient>>());
                mock.Setup(c => c.GetAddressesByLine1Async("abc", It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new[] { new AddressDto("1","A",null,"City",null,"Zip","Country",null,null,true,"PHYSICAL") });

                services.AddSingleton(mock.Object);
            });
        });
    }

    [Fact]
    public async Task GetAddresses_ReturnsOk()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-API-Key", "PUBLIC_API_KEY_PLACEHOLDER");

        var response = await client.GetAsync("/api/addresses?line1=abc");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
