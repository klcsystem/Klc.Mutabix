namespace Klc.Mutabix.Application.Common.Interfaces;

public interface IMailService
{
    Task SendMailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}
