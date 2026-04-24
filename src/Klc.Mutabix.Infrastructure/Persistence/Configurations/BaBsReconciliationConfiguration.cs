using Klc.Mutabix.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Klc.Mutabix.Infrastructure.Persistence.Configurations;

public class BaBsReconciliationConfiguration : IEntityTypeConfiguration<BaBsReconciliation>
{
    public void Configure(EntityTypeBuilder<BaBsReconciliation> builder)
    {
        builder.ToTable("babs_reconciliations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.Guid).HasMaxLength(100);
        builder.HasIndex(x => x.Guid).IsUnique().HasFilter("\"Guid\" IS NOT NULL");

        builder.HasMany(x => x.Details)
            .WithOne(x => x.BaBsReconciliation)
            .HasForeignKey(x => x.BaBsReconciliationId);
    }
}

public class BaBsReconciliationDetailConfiguration : IEntityTypeConfiguration<BaBsReconciliationDetail>
{
    public void Configure(EntityTypeBuilder<BaBsReconciliationDetail> builder)
    {
        builder.ToTable("babs_reconciliation_details");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Amount).HasPrecision(18, 2);
    }
}
