using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace Dotnet.CQRS.MediatR.EntityFrameworkCore;

public class TransactionBehavior<TRequest, TResponse>(IApplicationDbContext applicationDbContext)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IApplicationDbContext _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));

    public async Task<TResponse> Handle(TRequest request,
        [NotNull] RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_applicationDbContext.HasActiveTransaction)
        {
            return await next(cancellationToken);
        }

        if (request is ICommand)
        {
            return await HandleWithTransactionAsync(next);
        }

        return await HandleWithoutTransactionAsync(next);

    }

    private async Task<TResponse> HandleWithTransactionAsync(RequestHandlerDelegate<TResponse> next)
    {
        var response = default(TResponse);

        if (_applicationDbContext.HasActiveTransaction)
        {
            return await next();
        }

        var strategy = _applicationDbContext.GetStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _applicationDbContext.BeginTransactionAsync();
            response = await next();
            await _applicationDbContext.CommitTransactionAsync(transaction);
        });

        return response!;
    }

    private async Task<TResponse> HandleWithoutTransactionAsync(RequestHandlerDelegate<TResponse> next)
    {
        var response = default(TResponse);
        _applicationDbContext.SetQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        var strategy = _applicationDbContext.GetStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            response = await next();
        });

        return response!;
    }

}