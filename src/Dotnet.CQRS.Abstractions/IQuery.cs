namespace Dotnet.CQRS.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse> { }