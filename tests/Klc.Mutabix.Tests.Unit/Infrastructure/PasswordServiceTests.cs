using FluentAssertions;
using Klc.Mutabix.Infrastructure.Services;

namespace Klc.Mutabix.Tests.Unit.Infrastructure;

public class PasswordServiceTests
{
    private readonly PasswordService _service = new();

    [Fact]
    public void CreatePasswordHash_ShouldProduceNonEmptyHashAndSalt()
    {
        _service.CreatePasswordHash("TestPassword", out byte[] hash, out byte[] salt);

        hash.Should().NotBeEmpty();
        salt.Should().NotBeEmpty();
    }

    [Fact]
    public void VerifyPasswordHash_WithCorrectPassword_ShouldReturnTrue()
    {
        _service.CreatePasswordHash("Dogru123", out byte[] hash, out byte[] salt);

        var result = _service.VerifyPasswordHash("Dogru123", hash, salt);

        result.Should().BeTrue();
    }

    [Fact]
    public void VerifyPasswordHash_WithWrongPassword_ShouldReturnFalse()
    {
        _service.CreatePasswordHash("Dogru123", out byte[] hash, out byte[] salt);

        var result = _service.VerifyPasswordHash("Yanlis456", hash, salt);

        result.Should().BeFalse();
    }

    [Fact]
    public void CreatePasswordHash_ShouldProduceDifferentSaltEachTime()
    {
        _service.CreatePasswordHash("SamePassword", out byte[] hash1, out byte[] salt1);
        _service.CreatePasswordHash("SamePassword", out byte[] hash2, out byte[] salt2);

        salt1.Should().NotEqual(salt2);
        hash1.Should().NotEqual(hash2);
    }

    [Fact]
    public void VerifyPasswordHash_WithWrongSalt_ShouldReturnFalse()
    {
        _service.CreatePasswordHash("Password1", out byte[] hash1, out byte[] salt1);
        _service.CreatePasswordHash("Password2", out _, out byte[] salt2);

        var result = _service.VerifyPasswordHash("Password1", hash1, salt2);

        result.Should().BeFalse();
    }
}
