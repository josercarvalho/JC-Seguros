using Application.Dtos;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class VeiculoApplicationService
{
    private readonly IVeiculoRepository _veiculoRepository;

    public VeiculoApplicationService(IVeiculoRepository veiculoRepository)
    {
        _veiculoRepository = veiculoRepository;
    }

    public async Task<Guid> CreateVeiculoAsync(decimal valor, string marcaModelo)
    {
        var veiculo = Veiculo.CriarVeiculo(valor, marcaModelo);
        await _veiculoRepository.AddAsync(veiculo); 
        return veiculo.Id;
    }

    public async Task<VeiculoDto> GetVeiculoByIdAsync(Guid id)
    {
        var veiculo = await _veiculoRepository.GetByIdAsync(id); 
        if (veiculo == null) return null;

        return new VeiculoDto
        {
            Id = veiculo.Id,
            Valor = veiculo.Valor,
            MarcaModelo = veiculo.MarcaModelo
        };
    }

    public async Task UpdateVeiculoAsync(Guid id, decimal valor, string marcaModelo)
    {
        var veiculo = await _veiculoRepository.GetByIdAsync(id);
        if (veiculo == null) throw new KeyNotFoundException($"Veículo com Id {id} não encontrado.");

        veiculo.AtualizarDados(valor, marcaModelo);
        await _veiculoRepository.UpdateAsync(veiculo); 
    }

    public async Task DeleteVeiculoAsync(Guid id)
    {
        await _veiculoRepository.DeleteAsync(id); 
    }
}
