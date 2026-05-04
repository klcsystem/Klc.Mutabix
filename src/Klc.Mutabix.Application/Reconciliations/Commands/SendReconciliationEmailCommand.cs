using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Reconciliations.Commands;

public record SendReconciliationEmailCommand(
    int ReconciliationId,
    ReconciliationType Type,
    string BaseUrl = "https://mutabix.klcsystem.com",
    string? Cc = null) : IRequest<bool>;

public class SendReconciliationEmailCommandHandler(
    IApplicationDbContext context,
    IMailService mailService)
    : IRequestHandler<SendReconciliationEmailCommand, bool>
{
    public async Task<bool> Handle(
        SendReconciliationEmailCommand request, CancellationToken cancellationToken)
    {
        var baseUrl = request.BaseUrl.TrimEnd('/');

        if (request.Type == ReconciliationType.AccountReconciliation)
        {
            var rec = await context.AccountReconciliations
                .Include(r => r.CurrencyAccount)
                    .ThenInclude(ca => ca.Company)
                .FirstOrDefaultAsync(r => r.Id == request.ReconciliationId, cancellationToken);

            if (rec is null || string.IsNullOrEmpty(rec.CurrencyAccount.Email))
                return false;

            var company = rec.CurrencyAccount.Company;
            var approvalLink = $"{baseUrl}/reconciliation/respond/{rec.Guid}";
            var subject = $"Cari Hesap Mutabakat Talebi - {company.Name}";
            var body = BuildAccountReconciliationHtml(
                companyName: company.Name,
                companyTaxNumber: company.TaxNumber ?? "",
                companyAddress: company.Address ?? "",
                currencyAccountName: rec.CurrencyAccount.Name,
                startDate: rec.StartDate,
                endDate: rec.EndDate,
                debitAmount: rec.DebitAmount,
                creditAmount: rec.CreditAmount,
                currencyType: rec.CurrencyType.ToString(),
                approvalLink: approvalLink,
                reconciliationId: rec.Id);

            await mailService.SendMailAsync(rec.CurrencyAccount.Email, subject, body, request.Cc, cancellationToken);

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
                    .ThenInclude(ca => ca.Company)
                .FirstOrDefaultAsync(r => r.Id == request.ReconciliationId, cancellationToken);

            if (rec is null || string.IsNullOrEmpty(rec.CurrencyAccount.Email))
                return false;

            var company = rec.CurrencyAccount.Company;
            var approvalLink = $"{baseUrl}/reconciliation/respond/{rec.Guid}";
            var subject = $"Ba/Bs Mutabakat Talebi - {company.Name}";
            var body = BuildBaBsReconciliationHtml(
                companyName: company.Name,
                companyTaxNumber: company.TaxNumber ?? "",
                companyAddress: company.Address ?? "",
                currencyAccountName: rec.CurrencyAccount.Name,
                year: rec.Year,
                month: rec.Month,
                type: rec.Type.ToString(),
                amount: rec.Amount,
                quantity: rec.Quantity,
                approvalLink: approvalLink,
                reconciliationId: rec.Id);

            await mailService.SendMailAsync(rec.CurrencyAccount.Email, subject, body, request.Cc, cancellationToken);

            rec.IsSent = true;
            rec.SentDate = DateTime.UtcNow;
            rec.Status = ReconciliationStatus.Sent;
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    private static string BuildAccountReconciliationHtml(
        string companyName, string companyTaxNumber, string companyAddress,
        string currencyAccountName,
        DateTime startDate, DateTime endDate,
        decimal debitAmount, decimal creditAmount, string currencyType,
        string approvalLink, int reconciliationId)
    {
        var balance = debitAmount - creditAmount;
        var balanceLabel = balance >= 0 ? "Borc (Sizin Alacak)" : "Alacak (Sizin Borc)";

        return $"""
        <!DOCTYPE html>
        <html lang="tr">
        <head><meta charset="UTF-8" /></head>
        <body style="margin:0; padding:0; background-color:#f5f5f7; font-family: Arial, Helvetica, sans-serif;">
          <table width="100%" cellpadding="0" cellspacing="0" style="background-color:#f5f5f7; padding: 32px 0;">
            <tr>
              <td align="center">
                <table width="600" cellpadding="0" cellspacing="0" style="background-color:#ffffff; border-radius:12px; overflow:hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.06);">

                  <!-- Header -->
                  <tr>
                    <td style="background: linear-gradient(135deg, #1e293b 0%, #334155 100%); padding: 28px 32px;">
                      <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                          <td>
                            <h1 style="margin:0; color:#ffffff; font-size:22px; font-weight:700; letter-spacing:-0.5px;">Mutabix</h1>
                            <p style="margin:4px 0 0; color:#94a3b8; font-size:12px;">E-Mutabakat Platformu</p>
                          </td>
                          <td align="right">
                            <span style="display:inline-block; background-color:#f97316; color:#ffffff; font-size:11px; font-weight:700; padding:6px 14px; border-radius:20px; text-transform:uppercase; letter-spacing:0.5px;">Mutabakat Talebi</span>
                          </td>
                        </tr>
                      </table>
                    </td>
                  </tr>

                  <!-- Company Info -->
                  <tr>
                    <td style="padding: 28px 32px 0;">
                      <table width="100%" cellpadding="0" cellspacing="0" style="background-color:#f8fafc; border:1px solid #e2e8f0; border-radius:8px; padding:16px;">
                        <tr>
                          <td>
                            <p style="margin:0; font-size:11px; color:#94a3b8; text-transform:uppercase; letter-spacing:1px; font-weight:600;">Gonderen Firma</p>
                            <p style="margin:6px 0 0; font-size:16px; color:#0f172a; font-weight:700;">{companyName}</p>
                            <p style="margin:4px 0 0; font-size:12px; color:#64748b;">VKN: {companyTaxNumber}</p>
                            <p style="margin:2px 0 0; font-size:12px; color:#64748b;">{companyAddress}</p>
                          </td>
                        </tr>
                      </table>
                    </td>
                  </tr>

                  <!-- Greeting -->
                  <tr>
                    <td style="padding: 24px 32px 0;">
                      <h2 style="margin:0; font-size:20px; color:#0f172a; font-weight:700; font-style:italic;">Merhaba,</h2>
                    </td>
                  </tr>

                  <!-- Message -->
                  <tr>
                    <td style="padding: 16px 32px 0;">
                      <p style="margin:0; font-size:14px; color:#334155; line-height:1.7;">
                        <span style="color:#f97316; font-weight:700;">#{reconciliationId}</span> numarali formda
                        <strong>{currencyAccountName}</strong> ile aranizda tutari
                        <strong>{Math.Abs(balance):N2} {currencyType}</strong> {balanceLabel} olan mutabakat formu bulunmaktadir.
                      </p>
                    </td>
                  </tr>

                  <!-- Summary Table -->
                  <tr>
                    <td style="padding: 20px 32px 0;">
                      <table width="100%" cellpadding="0" cellspacing="0" style="border:1px solid #e2e8f0; border-radius:8px; overflow:hidden;">
                        <tr style="background-color:#f8fafc;">
                          <td style="padding:10px 16px; font-size:12px; color:#64748b; font-weight:600; border-bottom:1px solid #e2e8f0;">Donem</td>
                          <td style="padding:10px 16px; font-size:13px; color:#0f172a; font-weight:600; border-bottom:1px solid #e2e8f0; text-align:right;">{startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}</td>
                        </tr>
                        <tr>
                          <td style="padding:10px 16px; font-size:12px; color:#64748b; font-weight:600; border-bottom:1px solid #e2e8f0;">Borc</td>
                          <td style="padding:10px 16px; font-size:13px; color:#0f172a; font-weight:600; border-bottom:1px solid #e2e8f0; text-align:right;">{debitAmount:N2} {currencyType}</td>
                        </tr>
                        <tr>
                          <td style="padding:10px 16px; font-size:12px; color:#64748b; font-weight:600; border-bottom:1px solid #e2e8f0;">Alacak</td>
                          <td style="padding:10px 16px; font-size:13px; color:#0f172a; font-weight:600; border-bottom:1px solid #e2e8f0; text-align:right;">{creditAmount:N2} {currencyType}</td>
                        </tr>
                        <tr style="background-color:#fff7ed;">
                          <td style="padding:10px 16px; font-size:12px; color:#ea580c; font-weight:700;">Bakiye</td>
                          <td style="padding:10px 16px; font-size:14px; color:#ea580c; font-weight:700; text-align:right;">{Math.Abs(balance):N2} {currencyType}</td>
                        </tr>
                      </table>
                    </td>
                  </tr>

                  <!-- Instructions -->
                  <tr>
                    <td style="padding: 20px 32px 0;">
                      <p style="margin:0; font-size:13px; color:#64748b; line-height:1.6;">
                        Forma direkt ulasmak icin asagidaki linki tiklayabilir veya kopyalayip browser adres bolumune yapistirarak direkt ulasabilirsiniz.
                      </p>
                      <p style="margin:8px 0 0;">
                        <a href="{approvalLink}" style="font-size:12px; color:#2563eb; word-break:break-all;">{approvalLink}</a>
                      </p>
                    </td>
                  </tr>

                  <!-- CTA Button -->
                  <tr>
                    <td style="padding: 24px 32px;" align="center">
                      <a href="{approvalLink}" style="display:inline-block; background:linear-gradient(135deg, #f97316 0%, #ea580c 100%); color:#ffffff; font-size:14px; font-weight:700; padding:14px 36px; border-radius:8px; text-decoration:none; letter-spacing:0.3px; box-shadow: 0 4px 12px rgba(249,115,22,0.3);">
                        Formu Goruntule
                      </a>
                    </td>
                  </tr>

                  <!-- Footer -->
                  <tr>
                    <td style="background-color:#f8fafc; padding: 20px 32px; border-top:1px solid #e2e8f0;">
                      <p style="margin:0; font-size:11px; color:#94a3b8; line-height:1.5;">
                        Bu email <strong>{companyName}</strong> tarafindan Mutabix E-Mutabakat Platformu uzerinden gonderilmistir.
                        Herhangi bir sorunuz varsa lutfen gonderen firma ile iletisime geciniz.
                      </p>
                      <p style="margin:8px 0 0; font-size:11px; color:#cbd5e1;">
                        &copy; {DateTime.UtcNow.Year} Mutabix — mutabix.klcsystem.com
                      </p>
                    </td>
                  </tr>

                </table>
              </td>
            </tr>
          </table>
        </body>
        </html>
        """;
    }

    private static string BuildBaBsReconciliationHtml(
        string companyName, string companyTaxNumber, string companyAddress,
        string currencyAccountName,
        int year, int month, string type, decimal amount, int quantity,
        string approvalLink, int reconciliationId)
    {
        return $"""
        <!DOCTYPE html>
        <html lang="tr">
        <head><meta charset="UTF-8" /></head>
        <body style="margin:0; padding:0; background-color:#f5f5f7; font-family: Arial, Helvetica, sans-serif;">
          <table width="100%" cellpadding="0" cellspacing="0" style="background-color:#f5f5f7; padding: 32px 0;">
            <tr>
              <td align="center">
                <table width="600" cellpadding="0" cellspacing="0" style="background-color:#ffffff; border-radius:12px; overflow:hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.06);">

                  <!-- Header -->
                  <tr>
                    <td style="background: linear-gradient(135deg, #1e293b 0%, #334155 100%); padding: 28px 32px;">
                      <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                          <td>
                            <h1 style="margin:0; color:#ffffff; font-size:22px; font-weight:700; letter-spacing:-0.5px;">Mutabix</h1>
                            <p style="margin:4px 0 0; color:#94a3b8; font-size:12px;">E-Mutabakat Platformu</p>
                          </td>
                          <td align="right">
                            <span style="display:inline-block; background-color:#8b5cf6; color:#ffffff; font-size:11px; font-weight:700; padding:6px 14px; border-radius:20px; text-transform:uppercase; letter-spacing:0.5px;">Ba/Bs Mutabakat</span>
                          </td>
                        </tr>
                      </table>
                    </td>
                  </tr>

                  <!-- Company Info -->
                  <tr>
                    <td style="padding: 28px 32px 0;">
                      <table width="100%" cellpadding="0" cellspacing="0" style="background-color:#f8fafc; border:1px solid #e2e8f0; border-radius:8px; padding:16px;">
                        <tr>
                          <td>
                            <p style="margin:0; font-size:11px; color:#94a3b8; text-transform:uppercase; letter-spacing:1px; font-weight:600;">Gonderen Firma</p>
                            <p style="margin:6px 0 0; font-size:16px; color:#0f172a; font-weight:700;">{companyName}</p>
                            <p style="margin:4px 0 0; font-size:12px; color:#64748b;">VKN: {companyTaxNumber}</p>
                            <p style="margin:2px 0 0; font-size:12px; color:#64748b;">{companyAddress}</p>
                          </td>
                        </tr>
                      </table>
                    </td>
                  </tr>

                  <!-- Greeting -->
                  <tr>
                    <td style="padding: 24px 32px 0;">
                      <h2 style="margin:0; font-size:20px; color:#0f172a; font-weight:700; font-style:italic;">Merhaba,</h2>
                    </td>
                  </tr>

                  <!-- Message -->
                  <tr>
                    <td style="padding: 16px 32px 0;">
                      <p style="margin:0; font-size:14px; color:#334155; line-height:1.7;">
                        <span style="color:#8b5cf6; font-weight:700;">#{reconciliationId}</span> numarali formda
                        <strong>{currencyAccountName}</strong> ile aranizda
                        <strong>{year}/{month:D2}</strong> donemi icin Ba/Bs mutabakat talebi olusturulmustur.
                      </p>
                    </td>
                  </tr>

                  <!-- Summary Table -->
                  <tr>
                    <td style="padding: 20px 32px 0;">
                      <table width="100%" cellpadding="0" cellspacing="0" style="border:1px solid #e2e8f0; border-radius:8px; overflow:hidden;">
                        <tr style="background-color:#f8fafc;">
                          <td style="padding:10px 16px; font-size:12px; color:#64748b; font-weight:600; border-bottom:1px solid #e2e8f0;">Donem</td>
                          <td style="padding:10px 16px; font-size:13px; color:#0f172a; font-weight:600; border-bottom:1px solid #e2e8f0; text-align:right;">{year}/{month:D2}</td>
                        </tr>
                        <tr>
                          <td style="padding:10px 16px; font-size:12px; color:#64748b; font-weight:600; border-bottom:1px solid #e2e8f0;">Tur</td>
                          <td style="padding:10px 16px; font-size:13px; color:#0f172a; font-weight:600; border-bottom:1px solid #e2e8f0; text-align:right;">{type}</td>
                        </tr>
                        <tr>
                          <td style="padding:10px 16px; font-size:12px; color:#64748b; font-weight:600; border-bottom:1px solid #e2e8f0;">Tutar</td>
                          <td style="padding:10px 16px; font-size:13px; color:#0f172a; font-weight:600; border-bottom:1px solid #e2e8f0; text-align:right;">{amount:N2} TRY</td>
                        </tr>
                        <tr style="background-color:#faf5ff;">
                          <td style="padding:10px 16px; font-size:12px; color:#7c3aed; font-weight:700;">Adet</td>
                          <td style="padding:10px 16px; font-size:14px; color:#7c3aed; font-weight:700; text-align:right;">{quantity}</td>
                        </tr>
                      </table>
                    </td>
                  </tr>

                  <!-- Instructions -->
                  <tr>
                    <td style="padding: 20px 32px 0;">
                      <p style="margin:0; font-size:13px; color:#64748b; line-height:1.6;">
                        Forma direkt ulasmak icin asagidaki linki tiklayabilir veya kopyalayip browser adres bolumune yapistirarak direkt ulasabilirsiniz.
                      </p>
                      <p style="margin:8px 0 0;">
                        <a href="{approvalLink}" style="font-size:12px; color:#2563eb; word-break:break-all;">{approvalLink}</a>
                      </p>
                    </td>
                  </tr>

                  <!-- CTA Button -->
                  <tr>
                    <td style="padding: 24px 32px;" align="center">
                      <a href="{approvalLink}" style="display:inline-block; background:linear-gradient(135deg, #8b5cf6 0%, #7c3aed 100%); color:#ffffff; font-size:14px; font-weight:700; padding:14px 36px; border-radius:8px; text-decoration:none; letter-spacing:0.3px; box-shadow: 0 4px 12px rgba(139,92,246,0.3);">
                        Formu Goruntule
                      </a>
                    </td>
                  </tr>

                  <!-- Footer -->
                  <tr>
                    <td style="background-color:#f8fafc; padding: 20px 32px; border-top:1px solid #e2e8f0;">
                      <p style="margin:0; font-size:11px; color:#94a3b8; line-height:1.5;">
                        Bu email <strong>{companyName}</strong> tarafindan Mutabix E-Mutabakat Platformu uzerinden gonderilmistir.
                        Herhangi bir sorunuz varsa lutfen gonderen firma ile iletisime geciniz.
                      </p>
                      <p style="margin:8px 0 0; font-size:11px; color:#cbd5e1;">
                        &copy; {DateTime.UtcNow.Year} Mutabix — mutabix.klcsystem.com
                      </p>
                    </td>
                  </tr>

                </table>
              </td>
            </tr>
          </table>
        </body>
        </html>
        """;
    }
}
