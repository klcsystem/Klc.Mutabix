using Klc.Mutabix.Domain.Entities;
using Klc.Mutabix.Domain.Enums;

namespace Klc.Mutabix.Application.Common.Interfaces;

public record ErpCurrencyAccountData(
    string Code,
    string Name,
    string? TaxNumber,
    string? Email,
    CurrencyType CurrencyType,
    decimal DebitBalance,
    decimal CreditBalance);

public interface IErpAdapter
{
    ErpProviderType Provider { get; }
    Task<bool> TestConnectionAsync(ErpConnection connection, CancellationToken cancellationToken = default);
    Task<List<ErpCurrencyAccountData>> SyncCurrencyAccountsAsync(ErpConnection connection, CancellationToken cancellationToken = default);
}

public interface IErpAdapterFactory
{
    IErpAdapter GetAdapter(ErpProviderType provider);
}
