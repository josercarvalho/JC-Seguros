using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Dtos;

namespace Application.Services;

public class SeguradoApplicationService
{
    private readonly ISeguradoRepository _seguradoRepository; 

    public SeguradoApplicationService(ISeguradoRepository seguradoRepository)
    {
        _seguradoRepository = seguradoRepository;
    }

    public async Task<Guid> CreateSeguradoAsync(string nome, string cpf, int idade)
    {
        var existingSegurado = await _seguradoRepository.GetByCpfAsync(cpf); 
        if (existingSegurado != null)
        {
            throw new ArgumentException($"Já existe um segurado cadastrado com o CPF: {cpf}.");
        }

        var segurado = Segurado.CriarSegurado(nome, cpf, idade);
        await _seguradoRepository.AddAsync(segurado); 
        return segurado.Id;
    }

    public async Task<SeguradoDto> GetSeguradoByIdAsync(Guid id)
    {
        var segurado = await _seguradoRepository.GetByIdAsync(id); 
        if (segurado == null) return null;

        return new SeguradoDto
        {
            Id = segurado.Id,
            Nome = segurado.Nome,
            CPF = segurado.CPF,
            Idade = segurado.Idade
        };
    }

    public async Task UpdateSeguradoAsync(Guid id, string nome, string cpf, int idade)
    {
        var segurado = await _seguradoRepository.GetByIdAsync(id);
        if (segurado == null) throw new KeyNotFoundException($"Segurado com Id {id} não encontrado.");

        if (segurado.CPF != cpf)
        {
            var existingSegurado = await _seguradoRepository.GetByCpfAsync(cpf);
            if (existingSegurado != null && existingSegurado.Id != id)
            {
                throw new ArgumentException($"Já existe outro segurado cadastrado com o CPF: {cpf}.");
            }
        }

        segurado.AtualizarDados(nome, cpf, idade);
        await _seguradoRepository.UpdateAsync(segurado); 
    }

    public async Task DeleteSeguradoAsync(Guid id)
    {
        await _seguradoRepository.DeleteAsync(id);
    }
}
