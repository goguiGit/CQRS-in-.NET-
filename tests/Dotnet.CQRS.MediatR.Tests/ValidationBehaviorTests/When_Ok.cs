using Dotnet.CQRS.Abstractions;
using Dotnet.CQRS.MediatR.Abstractions;
using Dotnet.CQRS.MediatR.Behaviors;
using Dotnet.CQRS.MediatR.Tests.ValidationBehaviorTests.Foo;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Dotnet.CQRS.MediatR.Tests.ValidationBehaviorTests;

public class When_Ok
{
    [Fact]
    public async Task Then_Ok()
    {
        // Arrange
        var sc = new ServiceCollection();
        var assembly = typeof(FooCommand).Assembly;
        
        sc.AddScoped<IRequestDispatcher, MediatorDispatcher>();
        
        sc.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
        });
        
        sc.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        sc.AddValidatorsFromAssembly(assembly);

        var sp = sc.BuildServiceProvider();
        var mediator = sp.GetRequiredService<IRequestDispatcher>();

        // Act
        var result = await mediator.Send(new FooCommand(1, "a"));
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();

    }
}