namespace Dotnet.CQRS.MediatR.Abstractions.Authorization;

public interface IRequestPermissionChecker<in T>
{
    Task AuthorizeAsync(T query, CancellationToken cancellation = default);
}