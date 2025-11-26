using MediatR;

namespace Dotnet.CQRS.MediatR.Abstractions;

public interface ICommand<out TResponse> : IRequest<TResponse>, ICommand;

public interface ICommand;