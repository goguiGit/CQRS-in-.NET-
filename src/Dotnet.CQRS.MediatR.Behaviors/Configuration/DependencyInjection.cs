using System.Reflection;
using Dotnet.CQRS.MediatR.Abstractions.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.CQRS.MediatR.Behaviors.Configuration;

public static class DependencyInjection
{
    public static void AddRequestPermissionFromAssembly(
        this IServiceCollection services,
        Assembly assembly,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
        => AddBehaviorsFromAssembly(services, assembly, typeof(IRequestPermissionChecker<>), lifetime);

    private static void AddBehaviorsFromAssembly(
        this IServiceCollection services,
        Assembly assembly,
        Type checkerType,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        assembly.GetTypesAssignableTo(checkerType).ForEach((type) =>
        {
            foreach (var implementedInterface in type.ImplementedInterfaces)
            {
                switch (lifetime)
                {
                    case ServiceLifetime.Scoped:
                        services.AddScoped(implementedInterface, type);
                        break;
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(implementedInterface, type);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(implementedInterface, type);
                        break;
                }
            }
        });
    }

    private static List<TypeInfo> GetTypesAssignableTo(this Assembly assembly, Type compareType)
    {
        var typeInfoList = assembly.DefinedTypes.Where(x => x is { IsClass: true, IsAbstract: false }
                                                            && x != compareType
                                                            && x.GetInterfaces()
                                                                .Any(i => i.IsGenericType
                                                                          && i.GetGenericTypeDefinition() == compareType)).ToList();

        return typeInfoList;
    }
}