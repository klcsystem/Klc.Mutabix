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

public class RegisterCommandTests : IDisposable
{
    private readonly MutabixDbContext _context;
    private readonly PasswordService _passwordService;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly RegisterCommandHandler _handler;
    private readonly RegisterCommandValidator _validator = new();

    public RegisterCommandTests()
    {
        var options = new DbContextOptionsBuilder<MutabixDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new MutabixDbContext(options);
        _passwordService = new PasswordService();
        _tokenServiceMock = new Mock<ITokenService>();
        _tokenServiceMock
            .Setup(t => t.CreateToken(It.IsAny<User>(), It.IsAny<List<OperationClaim>>()))
            .Returns("new-user-token");
        _handler = new RegisterCommandHandler(_context, _passwordService, _tokenServiceMock.Object);
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task Handle_WithValidData_ShouldRegisterAndReturnToken()
    {
        var dto = new RegisterDto("Yeni Kullanici", "yeni@test.com", "Sifre123", null);
        var command = new RegisterCommand(dto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Token.Should().Be("new-user-token");
        result.Expiration.Should().BeAfter(DateTime.UtcNow);

        var user = await _context.Users.FirstAsync();
        user.Name.Should().Be("Yeni Kullanici");
        user.Email.Should().Be("yeni@test.com");
        user.PasswordHash.Should().NotBeEmpty();
        user.PasswordSalt.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_WithExistingEmail_ShouldThrow()
    {
        // Seed existing user
        _passwordService.CreatePasswordHash("pass", out byte[] h, out byte[] s);
        _context.Users.Add(new User { Name = "Mevcut", Email = "mevcut@test.com", PasswordHash = h, PasswordSalt = s });
        await _context.SaveChangesAsync();

        var dto = new RegisterDto("Yeni", "mevcut@test.com", "Sifre123", null);
        var command = new RegisterCommand(dto);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*zaten kayitli*");
    }

    [Fact]
    public async Task Handle_WithCompanyId_ShouldLinkToCompany()
    {
        var company = new Company { Name = "Sirket A" };
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        var dto = new RegisterDto("Kullanici", "k@test.com", "Sifre123", company.Id);
        var command = new RegisterCommand(dto);

        await _handler.Handle(command, CancellationToken.None);

        var userCompany = await _context.UserCompanies.FirstOrDefaultAsync();
        userCompany.Should().NotBeNull();
        userCompany!.CompanyId.Should().Be(company.Id);
    }

    [Fact]
    public void Validate_WithEmptyName_ShouldFail()
    {
        var dto = new RegisterDto("", "a@b.com", "Sifre123", null);
        var command = new RegisterCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
    }

    [Fact]
    public void Validate_WithEmptyEmail_ShouldFail()
    {
        var dto = new RegisterDto("Test", "", "Sifre123", null);
        var command = new RegisterCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Email);
    }

    [Fact]
    public void Validate_WithEmptyPassword_ShouldFail()
    {
        var dto = new RegisterDto("Test", "a@b.com", "", null);
        var command = new RegisterCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Password);
    }

    [Fact]
    public void Validate_WithShortPassword_ShouldFail()
    {
        var dto = new RegisterDto("Test", "a@b.com", "12345", null);
        var command = new RegisterCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Password);
    }

    [Fact]
    public void Validate_WithInvalidEmail_ShouldFail()
    {
        var dto = new RegisterDto("Test", "gecersiz-email", "Sifre123", null);
        var command = new RegisterCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Dto.Email);
    }

    [Fact]
    public void Validate_WithValidData_ShouldPass()
    {
        var dto = new RegisterDto("Test", "test@test.com", "Sifre123", null);
        var command = new RegisterCommand(dto);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
