namespace Dotnet.CQRS.Abstractions;

public interface ICommand;

public interface ICommand<out TResponse> : ICommand;
