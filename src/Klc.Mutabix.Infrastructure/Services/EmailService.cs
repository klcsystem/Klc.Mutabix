using Klc.Mutabix.Application.Common.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Klc.Mutabix.Infrastructure.Services;

public class EmailService(IConfiguration configuration, ILogger<EmailService> logger) : IMailService
{
    public async Task SendMailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Email gonderiliyor -> To: {To}, Subject: {Subject}", to, subject);

        var smtpServer = configuration["Mail:SmtpServer"];
        if (string.IsNullOrEmpty(smtpServer))
        {
            logger.LogWarning("SMTP ayarlari yapilandirilmamis, email loglanarak atlanacak");
            return;
        }

        var smtpPort = int.Parse(configuration["Mail:SmtpPort"] ?? "1711");
        var useSsl = bool.Parse(configuration["Mail:UseSsl"] ?? "false");
        var fromAddress = configuration["Mail:FromAddress"] ?? "noreply@mutabix.com";
        var fromName = configuration["Mail:FromName"] ?? "Mutabix E-Mutabakat";

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, fromAddress));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(smtpServer, smtpPort, useSsl, cancellationToken);

        if (useSsl)
        {
            var username = configuration["Mail:Username"];
            var password = configuration["Mail:Password"];
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                await client.AuthenticateAsync(username, password, cancellationToken);
            }
        }

        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);

        logger.LogInformation("Email basariyla gonderildi -> To: {To}, Subject: {Subject}", to, subject);
    }
}
