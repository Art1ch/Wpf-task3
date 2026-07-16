using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TestApp.Application;

public static class Injection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        return services
            .AddValidators();
    }

    private static IServiceCollection AddValidators(this IServiceCollection services) =>
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
}
