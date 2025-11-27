using Dotnet.CQRS.Application.Employees.Queries.GetById;
using Dotnet.CQRS.Infrastructure;
using Dotnet.CQRS.MediatR.Behaviors;
using Dotnet.CQRS.MediatR.Behaviors.Configuration;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.CQRS.API.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDbContext<ApplicationDbContext>(options => options
                .UseInMemoryDatabase(databaseName: "DbTests"));

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = typeof(GetByIdQuery).Assembly;
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
        });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(assembly);
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPermissionCheckerBehavior<,>));
        services.AddRequestPermissionFromAssembly(assembly);

        return services;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();


        return services;
    }
}