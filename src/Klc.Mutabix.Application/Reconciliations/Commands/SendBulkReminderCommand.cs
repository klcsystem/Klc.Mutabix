using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Reconciliations.Commands;

public record SendBulkReminderCommand(int CompanyId, string BaseUrl = "https://mutabix.klcsystem.com") : IRequest<int>;

public class SendBulkReminderCommandHandler(
    IApplicationDbContext context,
    IMailService mailService)
    : IRequestHandler<SendBulkReminderCommand, int>
{
    public async Task<int> Handle(SendBulkReminderCommand request, CancellationToken cancellationToken)
    {
        var sentRecs = await context.AccountReconciliations
            .Include(r => r.CurrencyAccount)
            .Where(r => r.CompanyId == request.CompanyId
                     && r.IsActive
                     && r.Status == ReconciliationStatus.Sent
                     && r.SentDate.HasValue
                     && r.SentDate.Value < DateTime.UtcNow.AddDays(-7))
            .ToListAsync(cancellationToken);

        var reminderCount = 0;

        foreach (var rec in sentRecs)
        {
            if (string.IsNullOrEmpty(rec.CurrencyAccount.Email))
                continue;

            var baseUrl = request.BaseUrl.TrimEnd('/');
            var subject = $"Hatirlatma: Mutabakat Yaniti Bekleniyor - {rec.CurrencyAccount.Name}";
            var body = $"""
                Sayin Yetkili,

                {rec.CurrencyAccount.Name} hesabiniz icin gonderilen mutabakat talebine henuz yanit verilmemistir.

                Mutabakat onay linki: {baseUrl}/reconciliation/respond/{rec.Guid}

                Lutfen en kisa surede yanitinizi iletiniz.
                Saygilarimizla.
                """;

            await mailService.SendMailAsync(rec.CurrencyAccount.Email, subject, body, cancellationToken);
            reminderCount++;
        }

        return reminderCount;
    }
}
