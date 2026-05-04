using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Klc.Mutabix.Infrastructure.Persistence;

public static class MutabixSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MutabixDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<MutabixDbContext>>();

        await context.Database.EnsureCreatedAsync();

        if (await context.Companies.AnyAsync())
        {
            logger.LogInformation("Database already seeded");
            return;
        }

        logger.LogInformation("Seeding database...");

        // Currencies
        var currencies = new List<Currency>
        {
            new() { Code = "TRY", Name = "Turk Lirasi", Symbol = "\u20ba", CurrencyType = CurrencyType.TRY },
            new() { Code = "USD", Name = "Amerikan Dolari", Symbol = "$", CurrencyType = CurrencyType.USD },
            new() { Code = "EUR", Name = "Euro", Symbol = "\u20ac", CurrencyType = CurrencyType.EUR }
        };
        context.Currencies.AddRange(currencies);

        // Demo company
        var company = new Company
        {
            Name = "Demo Ticaret A.\u015e.",
            TaxNumber = "1234567890",
            TaxOffice = "Kadikoy",
            Address = "Istanbul, Turkiye"
        };
        context.Companies.Add(company);
        await context.SaveChangesAsync();

        // Operation claims
        var claims = new List<OperationClaim>
        {
            new() { Name = "Admin" },
            new() { Name = "Company.Read" },
            new() { Name = "Company.Write" },
            new() { Name = "Reconciliation.Read" },
            new() { Name = "Reconciliation.Write" },
            new() { Name = "Reconciliation.Send" }
        };
        context.OperationClaims.AddRange(claims);
        await context.SaveChangesAsync();

        // Admin user (password: Admin123!)
        byte[] passwordHash, passwordSalt;
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("Admin123!"));
        }

        var adminUser = new User
        {
            Name = "Admin Kullanici",
            Email = "admin@mutabix.com",
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        context.Users.Add(adminUser);
        await context.SaveChangesAsync();

        // User-Company link
        context.UserCompanies.Add(new UserCompany
        {
            UserId = adminUser.Id,
            CompanyId = company.Id
        });

        // Admin gets all claims
        foreach (var claim in claims)
        {
            context.UserOperationClaims.Add(new UserOperationClaim
            {
                UserId = adminUser.Id,
                OperationClaimId = claim.Id
            });
        }

        // 5 sample currency accounts
        var accounts = new List<CurrencyAccount>
        {
            new()
            {
                CompanyId = company.Id,
                Code = "120-001",
                Name = "ABC Tedarik Ltd.",
                TaxNumber = "9876543210",
                Email = "finans@abctedarik.com",
                CurrencyType = CurrencyType.TRY
            },
            new()
            {
                CompanyId = company.Id,
                Code = "120-002",
                Name = "XYZ Dis Ticaret A.S.",
                TaxNumber = "5432167890",
                Email = "muhasebe@xyzticaret.com",
                CurrencyType = CurrencyType.USD
            },
            new()
            {
                CompanyId = company.Id,
                Code = "120-003",
                Name = "Anadolu Lojistik",
                TaxNumber = "1122334455",
                Email = "info@anadolulojistik.com",
                CurrencyType = CurrencyType.TRY
            },
            new()
            {
                CompanyId = company.Id,
                Code = "320-001",
                Name = "Marmara Elektronik A.S.",
                TaxNumber = "6677889900",
                Email = "satis@marmaraelektronik.com",
                CurrencyType = CurrencyType.EUR
            },
            new()
            {
                CompanyId = company.Id,
                Code = "320-002",
                Name = "Ege Kimya San. Tic. Ltd.",
                TaxNumber = "3344556677",
                Email = "muhasebe@egekimya.com",
                CurrencyType = CurrencyType.TRY
            },
            new()
            {
                CompanyId = company.Id,
                Code = "120-006",
                Name = "KLC Sistem Bilgi Teknolojileri",
                TaxNumber = "0123456789",
                Email = "erkan.kilic@klcsystem.com",
                CurrencyType = CurrencyType.TRY
            },
            new()
            {
                CompanyId = company.Id,
                Code = "120-007",
                Name = "KLC SYSTEM",
                TaxNumber = "9876543211",
                Email = "erkan.kilic@klcsystem.com",
                CurrencyType = CurrencyType.TRY
            }
        };
        context.CurrencyAccounts.AddRange(accounts);

        // Default mail template
        context.MailTemplates.Add(new MailTemplate
        {
            CompanyId = company.Id,
            Type = MailTemplateType.AccountReconciliation,
            Subject = "Hesap Mutabakat Talebi - {{CompanyName}}",
            Body = """
                <!DOCTYPE html>
                <html>
                <body style="font-family: Arial, sans-serif; padding: 20px;">
                  <h2>Hesap Mutabakat Talebi</h2>
                  <p>Sayin Yetkili,</p>
                  <p><strong>{{CurrencyAccountName}}</strong> hesabiniz icin mutabakat talebi olusturulmustur.</p>
                  <table style="border-collapse: collapse; width: 100%; margin: 20px 0;">
                    <tr><td style="padding: 8px; border: 1px solid #ddd;"><strong>Donem</strong></td><td style="padding: 8px; border: 1px solid #ddd;">{{StartDate}} - {{EndDate}}</td></tr>
                    <tr><td style="padding: 8px; border: 1px solid #ddd;"><strong>Borc</strong></td><td style="padding: 8px; border: 1px solid #ddd;">{{DebitAmount}} {{Currency}}</td></tr>
                    <tr><td style="padding: 8px; border: 1px solid #ddd;"><strong>Alacak</strong></td><td style="padding: 8px; border: 1px solid #ddd;">{{CreditAmount}} {{Currency}}</td></tr>
                  </table>
                  <p><a href="{{ApprovalLink}}" style="background: #22c55e; color: white; padding: 12px 24px; text-decoration: none; border-radius: 4px;">Onayla</a>
                     <a href="{{RejectLink}}" style="background: #ef4444; color: white; padding: 12px 24px; text-decoration: none; border-radius: 4px; margin-left: 10px;">Reddet</a></p>
                  <p>Saygilarimizla,<br/>{{CompanyName}}</p>
                </body>
                </html>
                """
        });

        await context.SaveChangesAsync();
        logger.LogInformation("Database seeded successfully");
    }
}
