using Ardalis.Result;
using Dotnet.CQRS.Abstractions;
using MediatR;

namespace Dotnet.CQRS.MediatR.Tests.ValidationBehaviorTests.Foo;

public class FooCommandHandler : ICommandHandler<FooCommand, Result>, IRequestHandler<FooCommand, Result>
{
    public Task<Result> Handle(FooCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Success());
    }
}