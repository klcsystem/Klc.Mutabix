using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Application.Erp.Dtos;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Klc.Mutabix.Application.Erp.Commands;

public record SyncErpDataCommand(int ConnectionId) : IRequest<ErpSyncLogDto>;

public class SyncErpDataCommandHandler(
    IApplicationDbContext context,
    IErpAdapterFactory adapterFactory)
    : IRequestHandler<SyncErpDataCommand, ErpSyncLogDto>
{
    public async Task<ErpSyncLogDto> Handle(SyncErpDataCommand request, CancellationToken cancellationToken)
    {
        var connection = await context.ErpConnections
            .FirstOrDefaultAsync(c => c.Id == request.ConnectionId, cancellationToken)
            ?? throw new KeyNotFoundException("ERP baglantisi bulunamadi");

        var syncLog = new ErpSyncLog
        {
            ErpConnectionId = connection.Id,
            Status = SyncStatus.Running,
            StartedAt = DateTime.UtcNow
        };

        context.ErpSyncLogs.Add(syncLog);
        await context.SaveChangesAsync(cancellationToken);

        try
        {
            var adapter = adapterFactory.GetAdapter(connection.Provider);
            var data = await adapter.SyncCurrencyAccountsAsync(connection, cancellationToken);

            var processed = 0;
            var failed = 0;

            foreach (var item in data)
            {
                try
                {
                    var existing = await context.CurrencyAccounts
                        .FirstOrDefaultAsync(ca =>
                            ca.CompanyId == connection.CompanyId && ca.Code == item.Code, cancellationToken);

                    if (existing is null)
                    {
                        context.CurrencyAccounts.Add(new CurrencyAccount
                        {
                            CompanyId = connection.CompanyId,
                            Code = item.Code,
                            Name = item.Name,
                            TaxNumber = item.TaxNumber,
                            Email = item.Email,
                            CurrencyType = item.CurrencyType
                        });
                    }
                    else
                    {
                        existing.Name = item.Name;
                        existing.TaxNumber = item.TaxNumber;
                        existing.Email = item.Email;
                        existing.UpdatedAt = DateTime.UtcNow;
                    }
                    processed++;
                }
                catch
                {
                    failed++;
                }
            }

            await context.SaveChangesAsync(cancellationToken);

            syncLog.Status = SyncStatus.Completed;
            syncLog.RecordsProcessed = processed;
            syncLog.RecordsFailed = failed;
            syncLog.CompletedAt = DateTime.UtcNow;

            connection.LastSyncAt = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            syncLog.Status = SyncStatus.Failed;
            syncLog.ErrorMessage = ex.Message;
            syncLog.CompletedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync(cancellationToken);

        return new ErpSyncLogDto(
            syncLog.Id, syncLog.ErpConnectionId, syncLog.Status,
            syncLog.StartedAt, syncLog.CompletedAt,
            syncLog.RecordsProcessed, syncLog.RecordsFailed, syncLog.ErrorMessage);
    }
}
