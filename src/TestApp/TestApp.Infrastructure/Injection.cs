using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestApp.Application.Abstractions;
using TestApp.Infrastructure.Context;
using TestApp.Infrastructure.Exporters;
using TestApp.Infrastructure.Parsers;
using TestApp.Infrastructure.Repository;
using TestApp.Infrastructure.Settings;

namespace TestApp.Infrastructure;

public static class Injection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, UserDbSettings userDbSettings)
    {
        return services
            .AddUserContext(userDbSettings)
            .AddRepositories()
            .AddParsers()
            .AddExporters();
    }

    private static IServiceCollection AddUserContext(this IServiceCollection services, UserDbSettings userDbSettings) =>
        services.AddDbContext<UserContext>(x => x.UseSqlServer(userDbSettings.ConnectionString));

    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services.AddTransient<IUserRepository, UserRepository>();

    private static IServiceCollection AddParsers(this IServiceCollection services) =>
        services.AddTransient<IUserParser, CsvUserParser>();

    private static IServiceCollection AddExporters(this IServiceCollection services) =>
        services
            .AddTransient<IUserExporter, ExcelUserExporter>()
            .AddTransient<IUserExporter, XmlUserExporter>();

}
