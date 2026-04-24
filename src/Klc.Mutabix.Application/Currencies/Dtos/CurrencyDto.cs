using Klc.Mutabix.Domain.Enums;

namespace Klc.Mutabix.Application.Currencies.Dtos;

public record CurrencyDto(int Id, string Code, string Name, string Symbol, CurrencyType CurrencyType);
