using Ardalis.Result;
using Dotnet.CQRS.Abstractions;
using MediatR;

namespace Dotnet.CQRS.MediatR.Tests.ValidationBehaviorTests.Foo;

public record FooCommand(int IntValue, string StringValue) : ICommand<Result>, IRequest<Result>;
