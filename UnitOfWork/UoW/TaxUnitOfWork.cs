using TaxManager.Models;
using UnitOfWork.Interfaces;
using UnitOfWork.Repositories;

namespace UnitOfWork.UoW;

public class TaxUnitOfWork : ITaxUnitOfWork
{
    private TaxContext _context;
    private Dictionary<Type, object> _repositories;


    public TaxUnitOfWork(TaxContext taxContext)
    {
        _context = taxContext;
        _repositories = new Dictionary<Type, object>();
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        if (_repositories.ContainsKey(typeof(TEntity)))
        {
            return (IRepository<TEntity>)_repositories[typeof(TEntity)];
        }

        var repository = new Repository<TEntity>(_context);
        _repositories.Add(typeof(TEntity), repository);

        return repository;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
