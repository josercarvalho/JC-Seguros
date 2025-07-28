using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SeguradoRepository : RepositoryBase<Segurado>, ISeguradoRepository
{
    public SeguradoRepository(SeguroDbContext context) : base(context)
    {
    }

    public async Task<Segurado> GetByCpfAsync(string cpf)
    {
        return await _context.Segurados.AsNoTracking().FirstOrDefaultAsync(s => s.CPF == cpf);
    }
}
