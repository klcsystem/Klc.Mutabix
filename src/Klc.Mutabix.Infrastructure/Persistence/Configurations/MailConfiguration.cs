using Klc.Mutabix.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Klc.Mutabix.Infrastructure.Persistence.Configurations;

public class MailParameterConfiguration : IEntityTypeConfiguration<MailParameter>
{
    public void Configure(EntityTypeBuilder<MailParameter> builder)
    {
        builder.ToTable("mail_parameters");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.SmtpServer).IsRequired().HasMaxLength(200);
        builder.Property(x => x.SenderEmail).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Password).IsRequired().HasMaxLength(200);
    }
}

public class MailTemplateConfiguration : IEntityTypeConfiguration<MailTemplate>
{
    public void Configure(EntityTypeBuilder<MailTemplate> builder)
    {
        builder.ToTable("mail_templates");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Subject).IsRequired().HasMaxLength(300);
        builder.Property(x => x.Body).IsRequired();
    }
}
