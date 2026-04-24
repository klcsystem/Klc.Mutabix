using FluentAssertions;
using FluentValidation.TestHelper;
using Klc.Mutabix.Application.Auth.Commands;
using Klc.Mutabix.Application.Auth.Dtos;
using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Infrastructure.Persistence;
using Klc.Mutabix.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Klc.Mutabix.Tests.Unit.Application.Auth;

public class LoginCommandTests : IDisposable
{
    private readonly MutabixDbContext _context;
    private readonly PasswordService _passwordService;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly LoginCommandHandler _handler;
    private readonly LoginCommandValidator _validator = new();

    public LoginCommandTests()
    {
        var options = new DbContextOptionsBuilder<MutabixDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new MutabixDbContext(options);
        _passwordService = new PasswordService();
        _tokenServiceMock = new Mock<ITokenService>();
        _tokenServiceMock
            .Setup(t => t.CreateToken(It.IsAny<User>(), It.IsAny<List<OperationClaim>>()))
            .Returns("test-jwt-token");
        _handler = new LoginCommandHandler(_context, _passwordService, _tokenServiceMock.Object);

        // Seed a user
        _passwordService.CreatePasswordHash("Test1234", out byte[] hash, out byte[] salt);
        _context.Users.Add(new User
        {
            Name = "Test User",
            Email = "test@test.com",
            PasswordHash = hash,
            PasswordSalt = salt
        });
        _context.SaveChanges();
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task Handle_WithValidCredentials_ShouldReturnToken()
    {
        var dto = new LoginDto("test@test.com", "Test1234");
        var command = new LoginCommand(dto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Token.Should().Be("test-jwt-token");
        result.Expiration.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task Handle_WithWrongEmail_ShouldThrowUnauthorized()
    {
        var dto = new LoginDto("wrong@test.com", "Test1234");
        var command = new LoginCommand(dto);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*Gecersiz*");
    }

    [Fact]
    public async Task Handle_WithWrongPassword_ShouldThrowUnauthorized()
    {
        var dto = new LoginDto("test@test.com", "WrongPassword");
        var command = new LoginCommand(dto);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*Gecersiz*");
    }

    [Fact]
    public void Validate_WithEmptyEmail_ShouldFail()
    {
        var dto = new LoginDto("", "Test1234");
        var command = new LoginCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Email);
    }

    [Fact]
    public void Validate_WithEmptyPassword_ShouldFail()
    {
        var dto = new LoginDto("test@test.com", "");
        var command = new LoginCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Password);
    }

    [Fact]
    public void Validate_WithInvalidEmail_ShouldFail()
    {
        var dto = new LoginDto("invalid-email", "Test1234");
        var command = new LoginCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Email);
    }

    [Fact]
    public void Validate_WithShortPassword_ShouldFail()
    {
        var dto = new LoginDto("test@test.com", "12345");
        var command = new LoginCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Password);
    }

    [Fact]
    public void Validate_WithValidData_ShouldPass()
    {
        var dto = new LoginDto("test@test.com", "Test1234");
        var command = new LoginCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
