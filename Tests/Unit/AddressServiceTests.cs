using Moq;
using MDUC_BE.Api.Models;
using MDUC_BE.Application.Services;
using MDUC_BE.Infrastructure.GraphQL;

namespace MDUC_BE.Tests.Unit;

public class AddressServiceTests
{
    [Fact]
    public async Task GetByLine1Async_ReturnsFromClient()
    {
        var expected = new[] { new AddressDto("1","A","B","City",null,"Zip","Country",null,null,true,"PHYSICAL") };
        var client = new Mock<SonarGraphQLClient>(new HttpClient(), Mock.Of<IConfiguration>(), Mock.Of<ILogger<SonarGraphQLClient>>());
        client.Setup(c => c.GetAddressesByLine1Async("abc", It.IsAny<CancellationToken>())).ReturnsAsync(expected);
        var service = new AddressService(client.Object);

        var result = await service.GetByLine1Async("abc");

        Assert.Single(result);
    }
}
