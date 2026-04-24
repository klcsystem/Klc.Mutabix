using ClosedXML.Excel;
using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Klc.Mutabix.Infrastructure.Erp;

public class ExcelErpAdapter(ILogger<ExcelErpAdapter> logger) : IErpAdapter
{
    public ErpProviderType Provider => ErpProviderType.Excel;

    public Task<bool> TestConnectionAsync(ErpConnection connection, CancellationToken cancellationToken = default)
    {
        // Excel doesn't have a persistent connection
        return Task.FromResult(true);
    }

    public Task<List<ErpCurrencyAccountData>> SyncCurrencyAccountsAsync(ErpConnection connection, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Excel dosyasindan cari hesap okuması");
        // Will be called with file upload, this is the adapter pattern stub
        return Task.FromResult(new List<ErpCurrencyAccountData>());
    }

    public List<ErpCurrencyAccountData> ParseExcelFile(Stream fileStream)
    {
        var result = new List<ErpCurrencyAccountData>();

        using var workbook = new XLWorkbook(fileStream);
        var worksheet = workbook.Worksheets.First();
        var rows = worksheet.RangeUsed()?.RowsUsed().Skip(1) ?? [];

        foreach (var row in rows)
        {
            var code = row.Cell(1).GetString();
            var name = row.Cell(2).GetString();
            var taxNumber = row.Cell(3).GetString();
            var email = row.Cell(4).GetString();
            var debit = row.Cell(5).GetDouble();
            var credit = row.Cell(6).GetDouble();

            if (!string.IsNullOrWhiteSpace(code))
            {
                result.Add(new ErpCurrencyAccountData(
                    code, name, taxNumber, email,
                    CurrencyType.TRY, (decimal)debit, (decimal)credit));
            }
        }

        logger.LogInformation("Excel'den {Count} cari hesap okundu", result.Count);
        return result;
    }
}
