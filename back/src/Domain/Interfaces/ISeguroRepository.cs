using Domain.Entities;

namespace Domain.Interfaces;

public interface ISeguroRepository : IRepositoryBase<Seguro>
{
    Task<Seguro> GetByIdWithDetailsAsync(Guid id);
    Task<IEnumerable<Seguro>> GetAllWithDetailsAsync();
}
