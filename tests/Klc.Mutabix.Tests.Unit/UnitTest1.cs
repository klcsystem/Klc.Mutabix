using Klc.Mutabix.Domain.Entities;
using FluentAssertions;

namespace Klc.Mutabix.Tests.Unit;

public class DomainEntityTests
{
    [Fact]
    public void Company_ShouldHaveDefaultValues()
    {
        var company = new Company();

        company.IsActive.Should().BeTrue();
        company.Name.Should().BeEmpty();
        company.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void User_ShouldHaveDefaultValues()
    {
        var user = new User();

        user.IsActive.Should().BeTrue();
        user.Email.Should().BeEmpty();
        user.PasswordHash.Should().BeEmpty();
        user.PasswordSalt.Should().BeEmpty();
    }
}
