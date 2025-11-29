using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Dotnet.CQRS.MediatR.EntityFrameworkCore;

public class EfCoreTransactionManager<TContext>(TContext context) : IApplicationDbContext
    where TContext : DbContext
{
    private IDbContextTransaction? _currentTransaction;

    public DbConnection GetDbConnection() => context.Database.GetDbConnection();

    public bool HasActiveTransaction => _currentTransaction != null;

    public void SetQueryTrackingBehavior(QueryTrackingBehavior trackingBehavior) => context.ChangeTracker.QueryTrackingBehavior = trackingBehavior;

    public IExecutionStrategy GetStrategy() => context.Database.CreateExecutionStrategy();

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            return _currentTransaction;
        }

        _currentTransaction = await context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null)
        {
            throw new ArgumentNullException(nameof(transaction));
        }

        if (transaction != _currentTransaction)
        {
            throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");
        }

        try
        {
            await transaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public TContext GetContext() => context;

}