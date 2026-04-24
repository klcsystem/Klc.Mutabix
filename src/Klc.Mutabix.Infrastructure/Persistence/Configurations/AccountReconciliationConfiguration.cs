using Klc.Mutabix.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Klc.Mutabix.Infrastructure.Persistence.Configurations;

public class AccountReconciliationConfiguration : IEntityTypeConfiguration<AccountReconciliation>
{
    public void Configure(EntityTypeBuilder<AccountReconciliation> builder)
    {
        builder.ToTable("account_reconciliations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.DebitAmount).HasPrecision(18, 2);
        builder.Property(x => x.CreditAmount).HasPrecision(18, 2);
        builder.Property(x => x.Guid).HasMaxLength(100);
        builder.HasIndex(x => x.Guid).IsUnique().HasFilter("\"Guid\" IS NOT NULL");

        builder.HasMany(x => x.Details)
            .WithOne(x => x.AccountReconciliation)
            .HasForeignKey(x => x.AccountReconciliationId);
    }
}

public class AccountReconciliationDetailConfiguration : IEntityTypeConfiguration<AccountReconciliationDetail>
{
    public void Configure(EntityTypeBuilder<AccountReconciliationDetail> builder)
    {
        builder.ToTable("account_reconciliation_details");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.DebitAmount).HasPrecision(18, 2);
        builder.Property(x => x.CreditAmount).HasPrecision(18, 2);
    }
}
