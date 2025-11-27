using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Dotnet.CQRS.Infrastructure.Data;

namespace Dotnet.CQRS.API.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseInMemoryDatabase(databaseName: "DbTests");

            // IMPORTANTE: Habilita validaciones de modelo con InMemory
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });

        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ApplicationDbContextInitializer>();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = typeof(GetByIdQuery).Assembly;
        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(assembly); });

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
        services.AddConfigureSwagger();

        return services;
    }

    public static IServiceCollection AddConfigureSwagger(this IServiceCollection services)
    {
        // Swagger documentation API
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dotnet.CQRS.API", Version = "v1" });
            c.CustomSchemaIds(type => type.ToString());

            c.DescribeAllParametersInCamelCase();
        });

        return services;
    }
}