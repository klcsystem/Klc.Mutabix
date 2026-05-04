using FluentAssertions;
using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Reconciliations.Commands;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;
using Klc.Mutabix.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Klc.Mutabix.Tests.Unit.Application.Reconciliations;

public class SendReconciliationEmailTests : IDisposable
{
    private readonly MutabixDbContext _context;
    private readonly Mock<IMailService> _mailServiceMock;
    private readonly SendReconciliationEmailCommandHandler _handler;

    public SendReconciliationEmailTests()
    {
        var options = new DbContextOptionsBuilder<MutabixDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new MutabixDbContext(options);
        _mailServiceMock = new Mock<IMailService>();
        _mailServiceMock
            .Setup(m => m.SendMailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _handler = new SendReconciliationEmailCommandHandler(_context, _mailServiceMock.Object);
    }

    public void Dispose() => _context.Dispose();

    private async Task<(int companyId, int accountId, int reconId)> SeedPendingReconciliation(string email = "cari@test.com")
    {
        var company = new Company { Name = "Test Sirket" };
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        var account = new CurrencyAccount
        {
            CompanyId = company.Id, Code = "120-01", Name = "Test Cari",
            Email = email, CurrencyType = CurrencyType.TRY
        };
        _context.CurrencyAccounts.Add(account);
        await _context.SaveChangesAsync();

        var recon = new AccountReconciliation
        {
            CompanyId = company.Id, CurrencyAccountId = account.Id,
            StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 3, 31),
            CurrencyType = CurrencyType.TRY, DebitAmount = 10000, CreditAmount = 8000,
            Status = ReconciliationStatus.Pending, Guid = System.Guid.NewGuid().ToString()
        };
        _context.AccountReconciliations.Add(recon);
        await _context.SaveChangesAsync();

        return (company.Id, account.Id, recon.Id);
    }

    [Fact]
    public async Task Handle_PendingReconciliation_ShouldSendEmailAndSetSent()
    {
        var (_, _, reconId) = await SeedPendingReconciliation();
        var command = new SendReconciliationEmailCommand(reconId, ReconciliationType.AccountReconciliation);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();

        var recon = await _context.AccountReconciliations.FirstAsync();
        recon.IsSent.Should().BeTrue();
        recon.SentDate.Should().NotBeNull();
        recon.Status.Should().Be(ReconciliationStatus.Sent);

        _mailServiceMock.Verify(m => m.SendMailAsync(
            "cari@test.com",
            It.Is<string>(s => s.Contains("Mutabakat")),
            It.IsAny<string>(),
            It.IsAny<string?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNoEmail_ShouldReturnFalse()
    {
        var (_, _, reconId) = await SeedPendingReconciliation(email: "");

        // Clear the email
        var account = await _context.CurrencyAccounts.FirstAsync();
        account.Email = null;
        await _context.SaveChangesAsync();

        var command = new SendReconciliationEmailCommand(reconId, ReconciliationType.AccountReconciliation);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeFalse();
        _mailServiceMock.Verify(m => m.SendMailAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WithNonExistentId_ShouldReturnFalse()
    {
        var command = new SendReconciliationEmailCommand(999, ReconciliationType.AccountReconciliation);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeFalse();
    }
}
