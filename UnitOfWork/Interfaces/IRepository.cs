using System.Linq.Expressions;

namespace UnitOfWork.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    public void Add(TEntity entity);

    ValueTask<TEntity> GetByIdAsync(int id);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<IEnumerable<TEntity>> FindByCondition(Expression<Func<TEntity, bool>> expression);

    public void Delete(int entityId);

    public void Update(TEntity entity);
}
