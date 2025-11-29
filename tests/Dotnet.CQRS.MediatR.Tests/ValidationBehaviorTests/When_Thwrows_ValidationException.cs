using Dotnet.CQRS.MediatR.Behaviors;
using Dotnet.CQRS.MediatR.Tests.ValidationBehaviorTests.Foo;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Dotnet.CQRS.MediatR.Tests.ValidationBehaviorTests;

public class When_Thwrows_ValidationException
{
    [Fact]
    public async Task Then_Ok()
    {
        // Arrange
        var sc = new ServiceCollection();
        var assembly = typeof(FooCommand).Assembly;

        sc.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
        });
        sc.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        sc.AddValidatorsFromAssembly(assembly);

        var sp = sc.BuildServiceProvider();
        var mediator = sp.GetRequiredService<IMediator>();

        // Act
        var act = async () => { await mediator.Send(new FooCommand(0, string.Empty)); };
        
        // Assert
        var expectedException  = await act.ShouldThrowAsync<ValidationException>();
        expectedException.ShouldNotBeNull();
        expectedException.Errors.Count().ShouldBe(2);


    }
}