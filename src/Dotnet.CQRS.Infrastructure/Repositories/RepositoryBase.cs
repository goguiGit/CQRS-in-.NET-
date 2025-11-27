using System.Linq.Expressions;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.CQRS.Infrastructure.Repositories;

public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : EntityBase
{
    protected readonly ApplicationDbContext Context;

    public RepositoryBase(ApplicationDbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public virtual TEntity Add(TEntity entity)
    {
        var result = Context.Set<TEntity>().Add(entity);

        return result.Entity;
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        Context.Set<TEntity>().AddRange(entities);
    }

    public virtual async Task<TEntity> FindAsync(TKey id)
    {
        return await Context.Set<TEntity>().FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await Context.Set<TEntity>().ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression)
    {
        var entities = Where(expression).AsEnumerable();

        return await Task.FromResult(entities);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification)
    {
        var entities = Where(specification).AsEnumerable();

        return await Task.FromResult(entities);
    }

    public void Remove(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        Context.Set<TEntity>().RemoveRange(entities);
    }

    public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> filter = null) => Where(filter).SingleAsync();

    public Task<TEntity> SingleAsync(ISpecification<TEntity> specification) => Where(specification).SingleAsync();

    public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> criteria) => Where(criteria).SingleOrDefaultAsync();

    public Task<TEntity> SingleOrDefaultAsync(ISpecification<TEntity> specification) => Where(specification).SingleOrDefaultAsync();

    public Task<TResult> SingleOrDefaultAsync<TResult>(ISpecification<TEntity, TResult> specification) where TResult : class
    {
        var evaluator = new SpecificationEvaluator();
        var query = evaluator.GetQuery(Context.Set<TEntity>().AsQueryable(), specification);
        return query.SingleOrDefaultAsync();
    }

    private IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> criteria, params Expression<Func<TEntity, object>>[]? includes)
    {
        var queryable = Context.Set<TEntity>().AsQueryable();

        if (includes != null)
        {
            queryable = includes.Aggregate(queryable, (current, include) => current.Include(include));
        }

        if (criteria != null)
        {
            queryable = queryable.Where(criteria);
        }

        return queryable;
    }

    private IQueryable<TEntity> Where(ISpecification<TEntity> specification)
    {
        var evaluator = new SpecificationEvaluator();
        return evaluator.GetQuery(Context.Set<TEntity>().AsQueryable(), specification);
    }
}