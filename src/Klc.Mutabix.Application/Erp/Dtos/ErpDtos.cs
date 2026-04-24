using Klc.Mutabix.Domain.Enums;

namespace Klc.Mutabix.Application.Erp.Dtos;

public record ErpConnectionDto(
    int Id,
    int CompanyId,
    ErpProviderType Provider,
    string Name,
    string? ApiUrl,
    bool HasApiKey,
    string? Username,
    DateTime? LastSyncAt,
    bool IsActive,
    DateTime CreatedAt);

public record CreateErpConnectionDto(
    int CompanyId,
    ErpProviderType Provider,
    string Name,
    string? ConnectionString,
    string? ApiUrl,
    string? ApiKey,
    string? Username,
    string? Password,
    string? ExtraSettings);

public record ErpSyncLogDto(
    int Id,
    int ErpConnectionId,
    SyncStatus Status,
    DateTime StartedAt,
    DateTime? CompletedAt,
    int RecordsProcessed,
    int RecordsFailed,
    string? ErrorMessage);
