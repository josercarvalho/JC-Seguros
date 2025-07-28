using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SeguroRepository : RepositoryBase<Seguro>, ISeguroRepository
{
    public SeguroRepository(SeguroDbContext context) : base(context)
    {
    }

    public override async Task<Seguro> GetByIdAsync(Guid id)
    {
        return await _dbSet
                     .Include(s => s.Veiculo)
                     .Include(s => s.Segurado)
                     .AsNoTracking()
                     .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Seguro> GetByIdWithDetailsAsync(Guid id)
    {
        return await _dbSet
                    .Include(s => s.Veiculo)
                    .Include(s => s.Segurado)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Id == id);
    }

    // Sobrescrevendo GetAllAsync se você quiser que o padrão para Seguro sempre inclua os detalhes
    public override async Task<IEnumerable<Seguro>> GetAllAsync()
    {
        return await _dbSet
                     .Include(s => s.Veiculo)
                     .Include(s => s.Segurado)
                     .AsNoTracking()
                     .ToListAsync();
    }

    public async Task<IEnumerable<Seguro>> GetAllWithDetailsAsync()
    {
        return await _dbSet
                    .Include(s => s.Veiculo)
                    .Include(s => s.Segurado)
                    .AsNoTracking()
                    .ToListAsync();
    }
}