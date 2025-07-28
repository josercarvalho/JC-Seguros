using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : Entity
{
    protected readonly SeguroDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public RepositoryBase(SeguroDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>(); // Obtém o DbSet específico para a entidade
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await _dbSet.AsNoTracking().Where(expression).ToListAsync();
    }

    public virtual async Task<TEntity> GetByIdAsync(Guid id)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }
}
