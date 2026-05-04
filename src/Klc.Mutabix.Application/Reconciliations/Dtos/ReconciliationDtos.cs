using Klc.Mutabix.Domain.Enums;

namespace Klc.Mutabix.Application.Reconciliations.Dtos;

public record AccountReconciliationDto(
    int Id,
    int CompanyId,
    int CurrencyAccountId,
    string CurrencyAccountName,
    string? CurrencyAccountEmail,
    DateTime StartDate,
    DateTime EndDate,
    CurrencyType CurrencyType,
    decimal DebitAmount,
    decimal CreditAmount,
    ReconciliationStatus Status,
    string? Guid,
    bool IsSent,
    DateTime? SentDate,
    DateTime CreatedAt);

public record CreateAccountReconciliationDto(
    int CompanyId,
    int CurrencyAccountId,
    DateTime StartDate,
    DateTime EndDate,
    CurrencyType CurrencyType,
    decimal DebitAmount,
    decimal CreditAmount);

public record BaBsReconciliationDto(
    int Id,
    int CompanyId,
    int CurrencyAccountId,
    string CurrencyAccountName,
    BaBsType Type,
    int Year,
    int Month,
    decimal Amount,
    int Quantity,
    ReconciliationStatus Status,
    string? Guid,
    bool IsSent,
    DateTime? SentDate,
    DateTime CreatedAt);

public record CreateBaBsReconciliationDto(
    int CompanyId,
    int CurrencyAccountId,
    BaBsType Type,
    int Year,
    int Month,
    decimal Amount,
    int Quantity);

public record ReconciliationStatsDto(
    int TotalAccountReconciliations,
    int PendingAccountReconciliations,
    int ApprovedAccountReconciliations,
    int RejectedAccountReconciliations,
    int TotalBaBsReconciliations,
    int PendingBaBsReconciliations,
    int ApprovedBaBsReconciliations,
    int RejectedBaBsReconciliations);

public record RespondToReconciliationDto(
    bool IsApproved,
    string? Note);
