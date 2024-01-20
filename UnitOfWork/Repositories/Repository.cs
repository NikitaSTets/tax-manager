using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UnitOfWork.Interfaces;

namespace UnitOfWork.Repositories;

public class Repository<TEntity> : IDisposable, IRepository<TEntity> where TEntity : class
{
    private bool disposed = false;
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }


    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public ValueTask<TEntity> GetByIdAsync(int id)
    {
        return _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var entities = await _dbSet.ToListAsync();

        return entities;
    }

    public async Task<IEnumerable<TEntity>> FindByCondition(Expression<Func<TEntity, bool>> expression)
    {
        var entities = await _dbSet.Where(expression).ToListAsync();

        return entities;
    }

    public void Delete(int entityId)
    {
        var entity = _dbSet.Find(entityId);
        _dbSet.Remove(entity);
    }

    public void Update(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

}