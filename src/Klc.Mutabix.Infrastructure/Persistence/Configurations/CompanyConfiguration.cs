using Klc.Mutabix.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Klc.Mutabix.Infrastructure.Persistence.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("companies");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.TaxNumber).HasMaxLength(20);
        builder.Property(x => x.TaxOffice).HasMaxLength(100);
        builder.Property(x => x.Address).HasMaxLength(500);

        builder.HasMany(x => x.UserCompanies)
            .WithOne(x => x.Company)
            .HasForeignKey(x => x.CompanyId);

        builder.HasMany(x => x.CurrencyAccounts)
            .WithOne(x => x.Company)
            .HasForeignKey(x => x.CompanyId);
    }
}
