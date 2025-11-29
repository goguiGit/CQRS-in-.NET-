using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.CQRS.MediatR.EntityFrameworkCore.Configuration;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddTransactionBehavior<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        services.AddScoped<EfCoreTransactionManager<TDbContext>>();
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<EfCoreTransactionManager<TDbContext>>());
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        return services;
    }
}