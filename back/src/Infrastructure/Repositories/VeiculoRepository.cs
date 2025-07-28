using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class VeiculoRepository : RepositoryBase<Veiculo>, IVeiculoRepository
{
    public VeiculoRepository(SeguroDbContext context) : base(context)
    {
    }
}
