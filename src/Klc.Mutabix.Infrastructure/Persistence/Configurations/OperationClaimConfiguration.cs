using Klc.Mutabix.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Klc.Mutabix.Infrastructure.Persistence.Configurations;

public class OperationClaimConfiguration : IEntityTypeConfiguration<OperationClaim>
{
    public void Configure(EntityTypeBuilder<OperationClaim> builder)
    {
        builder.ToTable("operation_claims");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasMany(x => x.UserOperationClaims)
            .WithOne(x => x.OperationClaim)
            .HasForeignKey(x => x.OperationClaimId);
    }
}

public class UserOperationClaimConfiguration : IEntityTypeConfiguration<UserOperationClaim>
{
    public void Configure(EntityTypeBuilder<UserOperationClaim> builder)
    {
        builder.ToTable("user_operation_claims");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.UserId, x.OperationClaimId }).IsUnique();
    }
}

public class UserCompanyConfiguration : IEntityTypeConfiguration<UserCompany>
{
    public void Configure(EntityTypeBuilder<UserCompany> builder)
    {
        builder.ToTable("user_companies");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.UserId, x.CompanyId }).IsUnique();
    }
}
