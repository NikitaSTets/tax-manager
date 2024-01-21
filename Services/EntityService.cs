using Services.Interfaces;
using System.Linq.Expressions;
using UnitOfWork.Interfaces;

namespace Services;

public class EntityService<TEntity> : IEntityService<TEntity> where TEntity : class
{
    private readonly ITaxUnitOfWork _taxUnitOfWork;


    public EntityService(ITaxUnitOfWork taxUnitOfWork)
    {
        _taxUnitOfWork = taxUnitOfWork;
    }

    public async Task AddAsync(TEntity entity)
    {
        var entityRepository = _taxUnitOfWork.GetRepository<TEntity>();
        entityRepository.Add(entity);

        await _taxUnitOfWork.SaveAsync();
    }

    public Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var entityRepository = _taxUnitOfWork.GetRepository<TEntity>();
        var entites = entityRepository.GetAllAsync();

        return entites;
    }

    public Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> expression)
    {
        var entityRepository = _taxUnitOfWork.GetRepository<TEntity>();
        var entites = entityRepository.FindByCondition(expression);

        return entites;
    }
}
