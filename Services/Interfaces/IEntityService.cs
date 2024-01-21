using System.Linq.Expressions;

namespace Services.Interfaces;

public interface IEntityService<TEntity> where TEntity : class
{
    Task AddAsync(TEntity entity);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> expression);
}
