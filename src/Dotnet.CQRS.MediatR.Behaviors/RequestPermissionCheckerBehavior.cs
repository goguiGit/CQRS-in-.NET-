using Dotnet.CQRS.MediatR.Abstractions.Authorization;
using MediatR;

namespace Dotnet.CQRS.MediatR.Behaviors;

public class RequestPermissionCheckerBehavior<TRequest, TResponse>(
    IEnumerable<IRequestPermissionChecker<TRequest>> permissionCheckers)
    : IPipelineBehavior<TRequest, TResponse>
    where TResponse : notnull
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        foreach (var permissionChecker in permissionCheckers)
        {
            await permissionChecker.AuthorizeAsync(request, cancellationToken);
        }

        return await next(cancellationToken);
    }

}