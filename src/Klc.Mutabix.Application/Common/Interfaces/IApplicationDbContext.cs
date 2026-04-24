using Klc.Mutabix.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Company> Companies { get; }
    DbSet<User> Users { get; }
    DbSet<UserCompany> UserCompanies { get; }
    DbSet<OperationClaim> OperationClaims { get; }
    DbSet<UserOperationClaim> UserOperationClaims { get; }
    DbSet<CurrencyAccount> CurrencyAccounts { get; }
    DbSet<AccountReconciliation> AccountReconciliations { get; }
    DbSet<AccountReconciliationDetail> AccountReconciliationDetails { get; }
    DbSet<BaBsReconciliation> BaBsReconciliations { get; }
    DbSet<BaBsReconciliationDetail> BaBsReconciliationDetails { get; }
    DbSet<MailParameter> MailParameters { get; }
    DbSet<Currency> Currencies { get; }
    DbSet<MailTemplate> MailTemplates { get; }
    DbSet<ErpConnection> ErpConnections { get; }
    DbSet<ErpSyncLog> ErpSyncLogs { get; }
    DbSet<AuditLog> AuditLogs { get; }
    DbSet<Notification> Notifications { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
