namespace Klc.Mutabix.Domain.Entities;

using Klc.Mutabix.Domain.Common;
using Klc.Mutabix.Domain.Enums;

public class MailTemplate : BaseEntity
{
    public int CompanyId { get; set; }
    public MailTemplateType Type { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
