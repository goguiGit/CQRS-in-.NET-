using System.Linq.Expressions;
using Ardalis.Specification;
using Dotnet.CQRS.Domain;

namespace Dotnet.CQRS.Application.Repositories;

public interface IRepository<TEntity, in TKey> where TEntity : EntityBase
{
    TEntity Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    Task<TEntity> FindAsync(TKey id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression);
    Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
    Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> filter = null);
    Task<TEntity> SingleAsync(ISpecification<TEntity> specification);
    Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> criteria);
    Task<TEntity> SingleOrDefaultAsync(ISpecification<TEntity> specification);
    Task<TResult> SingleOrDefaultAsync<TResult>(ISpecification<TEntity, TResult> specification) where TResult : class;
}