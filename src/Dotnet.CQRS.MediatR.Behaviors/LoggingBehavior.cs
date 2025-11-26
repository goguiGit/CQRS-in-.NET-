using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using Dotnet.CQRS.MediatR.Abstractions.Authorization;

namespace Dotnet.CQRS.MediatR.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var stopwatch = Stopwatch.StartNew();

        logger.LogInformation("Handling {RequestName} at {DateTime}", requestName, DateTime.UtcNow);

        try
        {
            var response = await next(cancellationToken);

            stopwatch.Stop();

            logger.LogInformation("Handled {RequestName} in {ElapsedMilliseconds}ms", requestName, stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            logger.LogError(
                ex,
                "Error handling {RequestName} after {ElapsedMilliseconds}ms",
                requestName,
                stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}

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
