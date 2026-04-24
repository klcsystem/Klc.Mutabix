using Klc.Mutabix.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Klc.Mutabix.Infrastructure.Persistence.Configurations;

public class CurrencyAccountConfiguration : IEntityTypeConfiguration<CurrencyAccount>
{
    public void Configure(EntityTypeBuilder<CurrencyAccount> builder)
    {
        builder.ToTable("currency_accounts");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.TaxNumber).HasMaxLength(20);
        builder.Property(x => x.Email).HasMaxLength(200);
        builder.HasIndex(x => new { x.CompanyId, x.Code }).IsUnique();

        builder.HasMany(x => x.AccountReconciliations)
            .WithOne(x => x.CurrencyAccount)
            .HasForeignKey(x => x.CurrencyAccountId);

        builder.HasMany(x => x.BaBsReconciliations)
            .WithOne(x => x.CurrencyAccount)
            .HasForeignKey(x => x.CurrencyAccountId);
    }
}
