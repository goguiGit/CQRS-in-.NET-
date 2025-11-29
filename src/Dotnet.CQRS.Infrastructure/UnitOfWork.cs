using Dotnet.CQRS.Application.Interfaces;

namespace Dotnet.CQRS.Infrastructure;

public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public void Dispose() => _dbContext.Dispose();
}