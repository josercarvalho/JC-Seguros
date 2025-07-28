using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Interfaces;

public interface IRepositoryBase<TEntity> where TEntity : Entity
{
    Task<TEntity> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> expression);
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(Guid id);
}
