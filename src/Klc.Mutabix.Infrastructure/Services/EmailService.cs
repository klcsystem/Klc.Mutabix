using Klc.Mutabix.Application.Common.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Klc.Mutabix.Infrastructure.Services;

public class EmailService(IServiceProvider serviceProvider, ILogger<EmailService> logger) : IMailService
{
    public Task SendMailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
        => SendMailAsync(to, subject, body, cc: null, cancellationToken);

    public async Task SendMailAsync(string to, string subject, string body, string? cc = null, CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Email gonderiliyor -> To: {To}, CC: {Cc}, Subject: {Subject}", to, cc ?? "-", subject);

        string smtpServer;
        int smtpPort;
        bool useSsl;
        string fromAddress;
        string? password = null;

        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var mailParam = await context.MailParameters
                .FirstOrDefaultAsync(mp => mp.IsActive, cancellationToken);

            if (mailParam is not null)
            {
                smtpServer = mailParam.SmtpServer;
                smtpPort = mailParam.SmtpPort;
                useSsl = mailParam.UseSsl;
                fromAddress = mailParam.SenderEmail;
                password = mailParam.Password;
                logger.LogInformation("Mail parametreleri veritabanindan yuklendi (CompanyId: {CompanyId})", mailParam.CompanyId);
            }
            else
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                smtpServer = configuration["Mail:SmtpServer"] ?? string.Empty;
                if (string.IsNullOrEmpty(smtpServer))
                {
                    logger.LogWarning("SMTP ayarlari yapilandirilmamis, email loglanarak atlanacak");
                    return;
                }

                smtpPort = int.Parse(configuration["Mail:SmtpPort"] ?? "465");
                useSsl = bool.Parse(configuration["Mail:UseSsl"] ?? "true");
                fromAddress = configuration["Mail:FromAddress"] ?? "noreply@mutabix.com";
                password = configuration["Mail:Password"];
                logger.LogInformation("Mail parametreleri konfigurasyondan yuklendi (fallback)");
            }
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Mutabix E-Mutabakat", fromAddress));
        message.To.Add(MailboxAddress.Parse(to));

        if (!string.IsNullOrEmpty(cc))
        {
            foreach (var ccAddr in cc.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                message.Cc.Add(MailboxAddress.Parse(ccAddr));
            }
        }

        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient();
        var sslOptions = useSsl
            ? (smtpPort == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls)
            : SecureSocketOptions.None;
        await client.ConnectAsync(smtpServer, smtpPort, sslOptions, cancellationToken);

        if (!string.IsNullOrEmpty(password))
        {
            await client.AuthenticateAsync(fromAddress, password, cancellationToken);
        }

        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);

        logger.LogInformation("Email basariyla gonderildi -> To: {To}, CC: {Cc}, Subject: {Subject}", to, cc ?? "-", subject);
    }
}
