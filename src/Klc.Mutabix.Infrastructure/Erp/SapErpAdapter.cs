using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Klc.Mutabix.Infrastructure.Erp;

public class SapErpAdapter(ILogger<SapErpAdapter> logger) : IErpAdapter
{
    public ErpProviderType Provider => ErpProviderType.Sap;

    public Task<bool> TestConnectionAsync(ErpConnection connection, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("SAP baglanti testi: {Url}", connection.ApiUrl);
        // SAP RFC/BAPI integration placeholder
        return Task.FromResult(!string.IsNullOrEmpty(connection.ApiUrl));
    }

    public Task<List<ErpCurrencyAccountData>> SyncCurrencyAccountsAsync(ErpConnection connection, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("SAP cari hesap senkronizasyonu baslatildi");
        // SAP RFC call placeholder - will integrate with SAP .NET Connector
        return Task.FromResult(new List<ErpCurrencyAccountData>());
    }
}
