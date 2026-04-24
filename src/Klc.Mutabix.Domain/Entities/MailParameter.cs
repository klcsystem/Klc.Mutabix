namespace Klc.Mutabix.Domain.Entities;

using Klc.Mutabix.Domain.Common;

public class MailParameter : BaseEntity
{
    public int CompanyId { get; set; }
    public string SmtpServer { get; set; } = string.Empty;
    public int SmtpPort { get; set; } = 587;
    public string SenderEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool UseSsl { get; set; } = true;
}
