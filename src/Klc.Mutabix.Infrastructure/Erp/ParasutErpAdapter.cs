using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Klc.Mutabix.Infrastructure.Erp;

public class ParasutErpAdapter(ILogger<ParasutErpAdapter> logger) : IErpAdapter
{
    public ErpProviderType Provider => ErpProviderType.Parasut;

    public Task<bool> TestConnectionAsync(ErpConnection connection, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Parasut API baglanti testi: {Url}", connection.ApiUrl);
        return Task.FromResult(!string.IsNullOrEmpty(connection.ApiKey));
    }

    public Task<List<ErpCurrencyAccountData>> SyncCurrencyAccountsAsync(ErpConnection connection, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Parasut cari hesap senkronizasyonu baslatildi");
        // Parasut REST API v4 placeholder
        return Task.FromResult(new List<ErpCurrencyAccountData>());
    }
}
