using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Klc.Mutabix.Application.Reconciliations.Commands;

public record SendReconciliationEmailCommand(int ReconciliationId, ReconciliationType Type) : IRequest<bool>;

public class SendReconciliationEmailCommandHandler(
    IApplicationDbContext context,
    IMailService mailService,
    IConfiguration configuration)
    : IRequestHandler<SendReconciliationEmailCommand, bool>
{
    public async Task<bool> Handle(
        SendReconciliationEmailCommand request, CancellationToken cancellationToken)
    {
        var baseUrl = configuration["App:BaseUrl"]?.TrimEnd('/') ?? "https://mutabix.klcsystem.com";

        if (request.Type == ReconciliationType.AccountReconciliation)
        {
            var rec = await context.AccountReconciliations
                .Include(r => r.CurrencyAccount)
                .FirstOrDefaultAsync(r => r.Id == request.ReconciliationId, cancellationToken);

            if (rec is null || string.IsNullOrEmpty(rec.CurrencyAccount.Email))
                return false;

            var approvalLink = $"{baseUrl}/reconciliation/respond/{rec.Guid}";
            var subject = $"Hesap Mutabakat Talebi - {rec.StartDate:dd.MM.yyyy} / {rec.EndDate:dd.MM.yyyy}";
            var body = $"""
                Sayin Yetkili,

                {rec.CurrencyAccount.Name} hesabiniz icin mutabakat talebi olusturulmustur.

                Donem: {rec.StartDate:dd.MM.yyyy} - {rec.EndDate:dd.MM.yyyy}
                Borc: {rec.DebitAmount:N2} {rec.CurrencyType}
                Alacak: {rec.CreditAmount:N2} {rec.CurrencyType}

                Mutabakat onay linki: {approvalLink}

                Saygilarimizla.
                """;

            await mailService.SendMailAsync(rec.CurrencyAccount.Email, subject, body, cancellationToken);

            rec.IsSent = true;
            rec.SentDate = DateTime.UtcNow;
            rec.Status = ReconciliationStatus.Sent;
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
        else
        {
            var rec = await context.BaBsReconciliations
                .Include(r => r.CurrencyAccount)
                .FirstOrDefaultAsync(r => r.Id == request.ReconciliationId, cancellationToken);

            if (rec is null || string.IsNullOrEmpty(rec.CurrencyAccount.Email))
                return false;

            var approvalLink = $"{baseUrl}/reconciliation/respond/{rec.Guid}";
            var subject = $"Ba/Bs Mutabakat Talebi - {rec.Year}/{rec.Month:D2}";
            var body = $"""
                Sayin Yetkili,

                {rec.CurrencyAccount.Name} hesabiniz icin Ba/Bs mutabakat talebi olusturulmustur.

                Donem: {rec.Year}/{rec.Month:D2}
                Tur: {rec.Type}
                Tutar: {rec.Amount:N2}
                Adet: {rec.Quantity}

                Mutabakat onay linki: {approvalLink}

                Saygilarimizla.
                """;

            await mailService.SendMailAsync(rec.CurrencyAccount.Email, subject, body, cancellationToken);

            rec.IsSent = true;
            rec.SentDate = DateTime.UtcNow;
            rec.Status = ReconciliationStatus.Sent;
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
