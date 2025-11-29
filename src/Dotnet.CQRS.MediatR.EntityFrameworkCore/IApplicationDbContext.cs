using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Dotnet.CQRS.MediatR.EntityFrameworkCore;

public interface IApplicationDbContext
{
    Task<IDbContextTransaction> BeginTransactionAsync();
    void RollbackTransaction();
    Task CommitTransactionAsync(IDbContextTransaction transaction);
    bool HasActiveTransaction { get; }
    IExecutionStrategy GetStrategy();
    void SetQueryTrackingBehavior(QueryTrackingBehavior trackingBehavior);
    DbConnection GetDbConnection();
}