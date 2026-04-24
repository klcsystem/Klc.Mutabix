using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Klc.Mutabix.Infrastructure.Erp;

public class GenericErpAdapter(ILogger<GenericErpAdapter> logger) : IErpAdapter
{
    public ErpProviderType Provider => ErpProviderType.Generic;

    public Task<bool> TestConnectionAsync(ErpConnection connection, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Generic ERP baglanti testi: {Url}", connection.ApiUrl);
        return Task.FromResult(!string.IsNullOrEmpty(connection.ApiUrl));
    }

    public Task<List<ErpCurrencyAccountData>> SyncCurrencyAccountsAsync(ErpConnection connection, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Generic ERP senkronizasyonu baslatildi");
        // Generic REST API integration placeholder
        return Task.FromResult(new List<ErpCurrencyAccountData>());
    }
}
