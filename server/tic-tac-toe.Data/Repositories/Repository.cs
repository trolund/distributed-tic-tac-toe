using System.Linq.Expressions;
using Manufacturing.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Manufacturing.Data.Repositories;

public class Repository<TEntity, TGUID>(DbContext context) : IRepository<TEntity, TGUID>
    where TEntity : class
{
    public async Task<TEntity?> Get(TGUID id)
    {
        return await context.Set<TEntity>().FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        return await context.Set<TEntity>().ToListAsync();
    }

    public IEnumerable<TEntity?> Find(Expression<Func<TEntity, bool>> predicate)
    {
        return context.Set<TEntity>().Where(predicate);
    }

    public async Task<TEntity?> SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return await context.Set<TEntity>().SingleOrDefaultAsync(predicate);
    }

    public async Task<TEntity> Add(TEntity entity)
    {
        var createdEntity = await context.Set<TEntity>().AddAsync(entity);
        return createdEntity.Entity;
    }

    public async Task AddRange(IEnumerable<TEntity> entities)
    {
        await context.Set<TEntity>().AddRangeAsync(entities);
    }

    public void Remove(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        context.Set<TEntity>().RemoveRange(entities);
    }
}