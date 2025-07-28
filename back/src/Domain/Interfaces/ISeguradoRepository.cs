using Domain.Entities;

namespace Domain.Interfaces;

public interface ISeguradoRepository : IRepositoryBase<Segurado>
{
    Task<Segurado> GetByCpfAsync(string cpf);
}
