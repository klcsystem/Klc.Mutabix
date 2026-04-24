namespace Klc.Mutabix.Application.MailParameters.Dtos;

public record MailParameterDto(
    int Id,
    int CompanyId,
    string SmtpServer,
    int SmtpPort,
    string SenderEmail,
    string Password,
    bool UseSsl,
    bool IsActive,
    DateTime CreatedAt);

public record CreateMailParameterDto(
    int CompanyId,
    string SmtpServer,
    int SmtpPort,
    string SenderEmail,
    string Password,
    bool UseSsl);

public record UpdateMailParameterDto(
    string SmtpServer,
    int SmtpPort,
    string SenderEmail,
    string Password,
    bool UseSsl);
