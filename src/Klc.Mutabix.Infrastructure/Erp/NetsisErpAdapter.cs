using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Klc.Mutabix.Infrastructure.Erp;

public class NetsisErpAdapter(ILogger<NetsisErpAdapter> logger) : IErpAdapter
{
    public ErpProviderType Provider => ErpProviderType.Netsis;

    public Task<bool> TestConnectionAsync(ErpConnection connection, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Netsis baglanti testi: {ConnStr}", connection.ConnectionString);
        return Task.FromResult(!string.IsNullOrEmpty(connection.ConnectionString));
    }

    public Task<List<ErpCurrencyAccountData>> SyncCurrencyAccountsAsync(ErpConnection connection, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Netsis cari hesap senkronizasyonu baslatildi");
        // Netsis DB direct query placeholder
        return Task.FromResult(new List<ErpCurrencyAccountData>());
    }
}
