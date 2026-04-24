using Klc.Mutabix.Application.Common.Interfaces;
using Klc.Mutabix.Infrastructure.Erp;
using Klc.Mutabix.Infrastructure.Persistence;
using Klc.Mutabix.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Klc.Mutabix.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<AuditInterceptor>();

        services.AddDbContext<MutabixDbContext>((sp, options) =>
            options
                .UseNpgsql(
                    configuration.GetConnectionString("PostgreSQL"),
                    b => b.MigrationsAssembly(typeof(MutabixDbContext).Assembly.FullName))
                .AddInterceptors(sp.GetRequiredService<AuditInterceptor>()));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<MutabixDbContext>());

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IMailService, EmailService>();

        // ERP Adapters
        services.AddScoped<IErpAdapter, SapErpAdapter>();
        services.AddScoped<IErpAdapter, LogoErpAdapter>();
        services.AddScoped<IErpAdapter, NetsisErpAdapter>();
        services.AddScoped<IErpAdapter, ParasutErpAdapter>();
        services.AddScoped<IErpAdapter, ExcelErpAdapter>();
        services.AddScoped<IErpAdapter, GenericErpAdapter>();
        services.AddScoped<IErpAdapterFactory, ErpAdapterFactory>();

        return services;
    }
}
