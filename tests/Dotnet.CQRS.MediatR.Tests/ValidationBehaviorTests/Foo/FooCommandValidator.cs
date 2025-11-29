using FluentValidation;

namespace Dotnet.CQRS.MediatR.Tests.ValidationBehaviorTests.Foo;

public class FooCommandValidator : AbstractValidator<FooCommand>
{
    public FooCommandValidator()
    {
        RuleFor(x => x.IntValue).GreaterThan(0);
        RuleFor(x => x.StringValue).NotEmpty();
    }
}