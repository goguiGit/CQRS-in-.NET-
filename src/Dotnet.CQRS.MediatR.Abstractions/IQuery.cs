using MediatR;

namespace Dotnet.CQRS.MediatR.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>;