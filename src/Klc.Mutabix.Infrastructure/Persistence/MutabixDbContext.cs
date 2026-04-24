using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Infrastructure.Persistence;

public class MutabixDbContext(DbContextOptions<MutabixDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserCompany> UserCompanies => Set<UserCompany>();
    public DbSet<OperationClaim> OperationClaims => Set<OperationClaim>();
    public DbSet<UserOperationClaim> UserOperationClaims => Set<UserOperationClaim>();
    public DbSet<CurrencyAccount> CurrencyAccounts => Set<CurrencyAccount>();
    public DbSet<AccountReconciliation> AccountReconciliations => Set<AccountReconciliation>();
    public DbSet<AccountReconciliationDetail> AccountReconciliationDetails => Set<AccountReconciliationDetail>();
    public DbSet<BaBsReconciliation> BaBsReconciliations => Set<BaBsReconciliation>();
    public DbSet<BaBsReconciliationDetail> BaBsReconciliationDetails => Set<BaBsReconciliationDetail>();
    public DbSet<MailParameter> MailParameters => Set<MailParameter>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<MailTemplate> MailTemplates => Set<MailTemplate>();
    public DbSet<ErpConnection> ErpConnections => Set<ErpConnection>();
    public DbSet<ErpSyncLog> ErpSyncLogs => Set<ErpSyncLog>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MutabixDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
