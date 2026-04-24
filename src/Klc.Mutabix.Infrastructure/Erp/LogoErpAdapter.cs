using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Klc.Mutabix.Infrastructure.Erp;

public class LogoErpAdapter(ILogger<LogoErpAdapter> logger) : IErpAdapter
{
    public ErpProviderType Provider => ErpProviderType.Logo;

    public Task<bool> TestConnectionAsync(ErpConnection connection, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Logo Tiger/Go baglanti testi: {ConnStr}", connection.ConnectionString);
        return Task.FromResult(!string.IsNullOrEmpty(connection.ConnectionString));
    }

    public Task<List<ErpCurrencyAccountData>> SyncCurrencyAccountsAsync(ErpConnection connection, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Logo cari hesap senkronizasyonu baslatildi");
        // Logo Unity API / direct DB integration placeholder
        return Task.FromResult(new List<ErpCurrencyAccountData>());
    }
}
