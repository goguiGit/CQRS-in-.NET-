namespace Dotnet.CQRS.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}