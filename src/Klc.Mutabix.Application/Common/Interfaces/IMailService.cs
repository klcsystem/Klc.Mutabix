namespace Klc.Mutabix.Application.Common.Interfaces;

public interface IMailService
{
    Task SendMailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
    Task SendMailAsync(string to, string subject, string body, string? cc = null, CancellationToken cancellationToken = default);
}
