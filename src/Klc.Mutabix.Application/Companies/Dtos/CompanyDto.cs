namespace Klc.Mutabix.Application.Companies.Dtos;

public record CompanyDto(
    int Id,
    string Name,
    string? TaxNumber,
    string? TaxOffice,
    string? Address,
    bool IsActive,
    DateTime CreatedAt);

public record CreateCompanyDto(
    string Name,
    string? TaxNumber,
    string? TaxOffice,
    string? Address);

public record UpdateCompanyDto(
    string Name,
    string? TaxNumber,
    string? TaxOffice,
    string? Address);
