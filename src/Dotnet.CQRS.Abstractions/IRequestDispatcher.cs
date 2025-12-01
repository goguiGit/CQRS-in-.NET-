namespace Dotnet.CQRS.Abstractions;

public interface IRequestDispatcher
{
    Task<TResponse?> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}