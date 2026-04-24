using Klc.Mutabix.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Klc.Mutabix.Infrastructure.Persistence.Configurations;

public class ErpConnectionConfiguration : IEntityTypeConfiguration<ErpConnection>
{
    public void Configure(EntityTypeBuilder<ErpConnection> builder)
    {
        builder.ToTable("erp_connections");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.ConnectionString).HasMaxLength(500);
        builder.Property(x => x.ApiUrl).HasMaxLength(500);
        builder.Property(x => x.ApiKey).HasMaxLength(500);
        builder.Property(x => x.Username).HasMaxLength(100);
        builder.Property(x => x.Password).HasMaxLength(200);
        builder.Property(x => x.ExtraSettings).HasColumnType("text");

        builder.HasOne(x => x.Company)
            .WithMany()
            .HasForeignKey(x => x.CompanyId);

        builder.HasMany(x => x.SyncLogs)
            .WithOne(x => x.ErpConnection)
            .HasForeignKey(x => x.ErpConnectionId);
    }
}

public class ErpSyncLogConfiguration : IEntityTypeConfiguration<ErpSyncLog>
{
    public void Configure(EntityTypeBuilder<ErpSyncLog> builder)
    {
        builder.ToTable("erp_sync_logs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ErrorMessage).HasColumnType("text");
    }
}
