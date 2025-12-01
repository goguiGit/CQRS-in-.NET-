using Dotnet.CQRS.Abstractions;
using MediatR;
using System.Reflection;

namespace Dotnet.CQRS.MediatR.Abstractions;

public class MediatorDispatcher(IMediator mediator, IServiceProvider services) : IRequestDispatcher
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    private readonly IServiceProvider _services = services ?? throw new ArgumentNullException(nameof(services));

    public async Task<TResponse?> Send<TResponse>(CQRS.Abstractions.IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        // If request also implements MediatR, delegate directly
        if (request is  MediatR.Abstractions.IRequest<TResponse> mediatrRequest)
        {
            return await _mediator.Send(mediatrRequest, cancellationToken);
        }

        var requestType = request.GetType();

        // Query
        if (request is IQuery<TResponse>)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(requestType, typeof(TResponse));
            var handler = _services.GetService(handlerType) ?? throw new ArgumentException($"No handler registered for query type {requestType.FullName}", nameof(request));
            var method = handlerType.GetMethod("Handle", BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidOperationException("Handle method not found on handler.");
            var task = (Task<TResponse>)method.Invoke(handler, [request, cancellationToken])!;
            return await task;
        }

        // Command
        if (request is ICommand<TResponse>)
        {
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(requestType, typeof(TResponse));
            var handler = _services.GetService(handlerType) ?? throw new ArgumentException($"No handler registered for command type {requestType.FullName}", nameof(request));
            var method = handlerType.GetMethod("Handle", BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidOperationException("Handle method not found on handler.");
            var task = (Task<TResponse>)method.Invoke(handler, [request, cancellationToken])!;
            return await task;
        }

        throw new ArgumentException($"{requestType.Name} does not implement a supported CQRS request interface.", nameof(request));
    }
}
