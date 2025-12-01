using Ardalis.Result;
using Dotnet.CQRS.Abstractions;

namespace Dotnet.CQRS.MediatR.Tests.ValidationBehaviorTests.Foo;

public record FooCommand(int IntValue, string StringValue) : ICommand<Result>;
