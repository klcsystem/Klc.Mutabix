using Klc.Mutabix.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Klc.Mutabix.Infrastructure.Services;

public class EmailService(IConfiguration configuration, ILogger<EmailService> logger) : IMailService
{
    public async Task SendMailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        // In development, just log the email. In production, integrate MailKit/SMTP.
        logger.LogInformation(
            "Email gonderildi -> To: {To}, Subject: {Subject}", to, subject);

        var smtpServer = configuration["Mail:SmtpServer"];
        if (string.IsNullOrEmpty(smtpServer))
        {
            logger.LogWarning("SMTP ayarlari yapilandirilmamis, email loglanarak atlanacak");
            return;
        }

        // MailKit integration placeholder - will be activated when SMTP is configured
        await Task.CompletedTask;
    }
}
