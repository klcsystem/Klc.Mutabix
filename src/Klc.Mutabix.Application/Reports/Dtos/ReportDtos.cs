using Klc.Mutabix.Domain.Enums;

namespace Klc.Mutabix.Application.Reports.Dtos;

public record ReconciliationSummaryDto(
    int TotalReconciliations,
    int Pending,
    int Sent,
    int Approved,
    int Rejected,
    decimal TotalDebitAmount,
    decimal TotalCreditAmount);

public record MonthlyTrendDto(
    int Year,
    int Month,
    int AccountReconciliationCount,
    int BaBsReconciliationCount,
    int ApprovedCount,
    int RejectedCount);

public record CurrencyAccountSummaryDto(
    int Id,
    string Code,
    string Name,
    int ReconciliationCount,
    int ApprovedCount,
    int PendingCount,
    ReconciliationStatus LastStatus);
