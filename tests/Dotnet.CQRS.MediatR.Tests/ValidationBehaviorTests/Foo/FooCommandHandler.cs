using Ardalis.Result;
using Dotnet.CQRS.MediatR.Abstractions;

namespace Dotnet.CQRS.MediatR.Tests.ValidationBehaviorTests.Foo;

public class FooCommandHandler : ICommandHandler<FooCommand, Result>
{
    public Task<Result> Handle(FooCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Success());
    }
}