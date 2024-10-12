using System.Linq.Expressions;

namespace Manufacturing.Data.Repositories.Interface;

public interface IRepository<TEntity, TGUID> where TEntity : class
{
    // get objects
    Task<TEntity?> Get(TGUID id);
    Task<IEnumerable<TEntity>> GetAll();
    IEnumerable<TEntity?> Find(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity?> SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

    // create objects
    Task<TEntity> Add(TEntity entity);

    Task AddRange(IEnumerable<TEntity> entities);

    // remove objects 
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
}