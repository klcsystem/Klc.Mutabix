using FluentAssertions;
using Klc.Mutabix.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Klc.Mutabix.Tests.Integration;

public class HealthCheckTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HealthCheckTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                // Remove all EF-related registrations
                var descriptors = services
                    .Where(d => d.ServiceType.FullName?.Contains("EntityFramework") == true
                             || d.ServiceType == typeof(DbContextOptions<MutabixDbContext>)
                             || d.ServiceType == typeof(DbContextOptions)
                             || d.ServiceType == typeof(MutabixDbContext))
                    .ToList();
                foreach (var d in descriptors)
                    services.Remove(d);

                // Add InMemory database
                services.AddDbContext<MutabixDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            });
        });
    }

    [Fact]
    public async Task HealthEndpoint_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/health");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task AuthPing_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/auth/ping");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}
