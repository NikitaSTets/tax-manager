namespace UnitOfWork.Interfaces;

public interface ITaxUnitOfWork
{
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

    Task SaveAsync();
}
