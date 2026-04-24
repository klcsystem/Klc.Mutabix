using ClosedXML.Excel;
using FluentAssertions;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;
using Klc.Mutabix.Infrastructure.Erp;
using Microsoft.Extensions.Logging;
using Moq;

namespace Klc.Mutabix.Tests.Unit.Infrastructure.Erp;

public class ExcelErpAdapterTests
{
    private readonly ExcelErpAdapter _adapter;

    public ExcelErpAdapterTests()
    {
        _adapter = new ExcelErpAdapter(Mock.Of<ILogger<ExcelErpAdapter>>());
    }

    [Fact]
    public void Provider_ShouldBeExcel()
    {
        _adapter.Provider.Should().Be(ErpProviderType.Excel);
    }

    [Fact]
    public async Task TestConnectionAsync_ShouldAlwaysReturnTrue()
    {
        var connection = new ErpConnection { Provider = ErpProviderType.Excel, Name = "Test" };

        var result = await _adapter.TestConnectionAsync(connection);

        result.Should().BeTrue();
    }

    [Fact]
    public void ParseExcelFile_WithValidData_ShouldReturnAccounts()
    {
        using var stream = CreateTestExcel(new[]
        {
            ("120-01", "Cari A", "1111111111", "a@test.com", 10000.0, 8000.0),
            ("120-02", "Cari B", "2222222222", "b@test.com", 5000.0, 3000.0)
        });

        var result = _adapter.ParseExcelFile(stream);

        result.Should().HaveCount(2);
        result[0].Code.Should().Be("120-01");
        result[0].Name.Should().Be("Cari A");
        result[0].TaxNumber.Should().Be("1111111111");
        result[0].Email.Should().Be("a@test.com");
        result[0].DebitBalance.Should().Be(10000m);
        result[0].CreditBalance.Should().Be(8000m);
        result[0].CurrencyType.Should().Be(CurrencyType.TRY);

        result[1].Code.Should().Be("120-02");
        result[1].Name.Should().Be("Cari B");
    }

    [Fact]
    public void ParseExcelFile_WithEmptySheet_ShouldReturnEmptyList()
    {
        using var stream = CreateTestExcel(Array.Empty<(string, string, string, string, double, double)>());

        var result = _adapter.ParseExcelFile(stream);

        result.Should().BeEmpty();
    }

    [Fact]
    public void ParseExcelFile_SkipsRowsWithEmptyCode()
    {
        using var stream = CreateTestExcel(new[]
        {
            ("120-01", "Gecerli", "111", "a@t.com", 1000.0, 500.0),
            ("", "Bos Kod", "222", "b@t.com", 2000.0, 1000.0),
            ("120-03", "Gecerli 2", "333", "c@t.com", 3000.0, 1500.0)
        });

        var result = _adapter.ParseExcelFile(stream);

        result.Should().HaveCount(2);
        result[0].Code.Should().Be("120-01");
        result[1].Code.Should().Be("120-03");
    }

    [Fact]
    public async Task SyncCurrencyAccountsAsync_ShouldReturnEmptyList()
    {
        var connection = new ErpConnection { Provider = ErpProviderType.Excel, Name = "Test" };

        var result = await _adapter.SyncCurrencyAccountsAsync(connection);

        result.Should().BeEmpty();
    }

    private static MemoryStream CreateTestExcel(
        (string code, string name, string taxNo, string email, double debit, double credit)[] rows)
    {
        var stream = new MemoryStream();
        using (var workbook = new XLWorkbook())
        {
            var ws = workbook.Worksheets.Add("CariHesaplar");
            ws.Cell(1, 1).Value = "Kod";
            ws.Cell(1, 2).Value = "Ad";
            ws.Cell(1, 3).Value = "Vergi No";
            ws.Cell(1, 4).Value = "Email";
            ws.Cell(1, 5).Value = "Borc";
            ws.Cell(1, 6).Value = "Alacak";

            for (int i = 0; i < rows.Length; i++)
            {
                var row = rows[i];
                ws.Cell(i + 2, 1).Value = row.code;
                ws.Cell(i + 2, 2).Value = row.name;
                ws.Cell(i + 2, 3).Value = row.taxNo;
                ws.Cell(i + 2, 4).Value = row.email;
                ws.Cell(i + 2, 5).Value = row.debit;
                ws.Cell(i + 2, 6).Value = row.credit;
            }

            workbook.SaveAs(stream);
        }
        stream.Position = 0;
        return stream;
    }
}
