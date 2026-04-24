using FluentAssertions;
using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Domain.Enums;
using Klc.Mutabix.Infrastructure.Erp;
using Microsoft.Extensions.Logging;
using Moq;

namespace Klc.Mutabix.Tests.Unit.Infrastructure.Erp;

public class ErpAdapterFactoryTests
{
    private readonly ErpAdapterFactory _factory;

    public ErpAdapterFactoryTests()
    {
        var adapters = new List<IErpAdapter>
        {
            new SapErpAdapter(Mock.Of<ILogger<SapErpAdapter>>()),
            new LogoErpAdapter(Mock.Of<ILogger<LogoErpAdapter>>()),
            new NetsisErpAdapter(Mock.Of<ILogger<NetsisErpAdapter>>()),
            new ParasutErpAdapter(Mock.Of<ILogger<ParasutErpAdapter>>()),
            new ExcelErpAdapter(Mock.Of<ILogger<ExcelErpAdapter>>()),
            new GenericErpAdapter(Mock.Of<ILogger<GenericErpAdapter>>())
        };
        _factory = new ErpAdapterFactory(adapters);
    }

    [Theory]
    [InlineData(ErpProviderType.Sap)]
    [InlineData(ErpProviderType.Logo)]
    [InlineData(ErpProviderType.Netsis)]
    [InlineData(ErpProviderType.Parasut)]
    [InlineData(ErpProviderType.Excel)]
    [InlineData(ErpProviderType.Generic)]
    public void GetAdapter_ShouldReturnCorrectAdapterForEachProvider(ErpProviderType provider)
    {
        var adapter = _factory.GetAdapter(provider);

        adapter.Should().NotBeNull();
        adapter.Provider.Should().Be(provider);
    }

    [Fact]
    public void GetAdapter_Sap_ShouldReturnSapAdapter()
    {
        var adapter = _factory.GetAdapter(ErpProviderType.Sap);
        adapter.Should().BeOfType<SapErpAdapter>();
    }

    [Fact]
    public void GetAdapter_Logo_ShouldReturnLogoAdapter()
    {
        var adapter = _factory.GetAdapter(ErpProviderType.Logo);
        adapter.Should().BeOfType<LogoErpAdapter>();
    }

    [Fact]
    public void GetAdapter_Netsis_ShouldReturnNetsisAdapter()
    {
        var adapter = _factory.GetAdapter(ErpProviderType.Netsis);
        adapter.Should().BeOfType<NetsisErpAdapter>();
    }

    [Fact]
    public void GetAdapter_Parasut_ShouldReturnParasutAdapter()
    {
        var adapter = _factory.GetAdapter(ErpProviderType.Parasut);
        adapter.Should().BeOfType<ParasutErpAdapter>();
    }

    [Fact]
    public void GetAdapter_Excel_ShouldReturnExcelAdapter()
    {
        var adapter = _factory.GetAdapter(ErpProviderType.Excel);
        adapter.Should().BeOfType<ExcelErpAdapter>();
    }

    [Fact]
    public void GetAdapter_Generic_ShouldReturnGenericAdapter()
    {
        var adapter = _factory.GetAdapter(ErpProviderType.Generic);
        adapter.Should().BeOfType<GenericErpAdapter>();
    }

    [Fact]
    public void GetAdapter_WithUnknownProvider_ShouldThrowNotSupportedException()
    {
        var factory = new ErpAdapterFactory(new List<IErpAdapter>());

        var act = () => factory.GetAdapter(ErpProviderType.Sap);

        act.Should().Throw<NotSupportedException>()
            .WithMessage("*desteklenmiyor*");
    }

    [Fact]
    public void GetAdapter_WithEmptyAdapters_ShouldThrowForAnyProvider()
    {
        var factory = new ErpAdapterFactory([]);

        var act = () => factory.GetAdapter((ErpProviderType)99);

        act.Should().Throw<NotSupportedException>();
    }
}
