using Klc.Mutabix.Domain.Enums;

namespace Klc.Mutabix.Application.CurrencyAccounts.Dtos;

public record CurrencyAccountDto(
    int Id,
    int CompanyId,
    string Code,
    string Name,
    string? TaxNumber,
    string? Email,
    CurrencyType CurrencyType,
    bool IsActive,
    DateTime CreatedAt);

public record CreateCurrencyAccountDto(
    int CompanyId,
    string Code,
    string Name,
    string? TaxNumber,
    string? Email,
    CurrencyType CurrencyType);

public record UpdateCurrencyAccountDto(
    string Code,
    string Name,
    string? TaxNumber,
    string? Email,
    CurrencyType CurrencyType);
