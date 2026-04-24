using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Domain.Enums;

namespace Klc.Mutabix.Infrastructure.Erp;

public class ErpAdapterFactory(IEnumerable<IErpAdapter> adapters) : IErpAdapterFactory
{
    public IErpAdapter GetAdapter(ErpProviderType provider)
    {
        return adapters.FirstOrDefault(a => a.Provider == provider)
            ?? throw new NotSupportedException($"ERP provider {provider} desteklenmiyor");
    }
}
