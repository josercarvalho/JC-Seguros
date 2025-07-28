using Application.Dtos;
using Application.Services;
using Application.Validators;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Services;

public class SeguroApplicationService : ISeguroApplicationService
{
    private readonly ISeguroRepository _seguroRepository;

    public SeguroApplicationService(ISeguroRepository seguroRepository )
    {
        _seguroRepository = seguroRepository;
    }

    public async Task<SeguroResponseDto> RegistrarNovoSeguroAsync(
    decimal valorVeiculo, string marcaModeloVeiculo,
    string nomeSegurado, string cpfSegurado, int idadeSegurado)
    {        
        var veiculo = Veiculo.CriarVeiculo(valorVeiculo, marcaModeloVeiculo);
        var segurado = Segurado.CriarSegurado(nomeSegurado, cpfSegurado, idadeSegurado);

        var seguro = Seguro.CriarSeguro(veiculo, segurado);

        await _seguroRepository.AddAsync(seguro); 

        return new SeguroResponseDto
        {
            Id = seguro.Id,
            TaxaRisco = seguro.TaxaRisco,
            PremioRisco = seguro.PremioRisco,
            PremioPuro = seguro.PremioPuro,
            PremioComercial = seguro.PremioComercial,
            Veiculo = new VeiculoDto
            {
                Id = seguro.Veiculo.Id,
                Valor = seguro.Veiculo.Valor,
                MarcaModelo = seguro.Veiculo.MarcaModelo
            },
            Segurado = new SeguradoDto
            {
                Id = seguro.Segurado.Id,
                Nome = seguro.Segurado.Nome,
                CPF = seguro.Segurado.CPF,
                Idade = seguro.Segurado.Idade
            }
        };
    }

    public async Task<SeguroResponseDto> ObterSeguroPorIdAsync(Guid id)
    {
        var seguro = await _seguroRepository.GetByIdWithDetailsAsync(id);
        if (seguro == null)
        {
            DomainExceptionValidation.When(true, $"Seguro com Id {id} não encontrado.");
            //return null;
        }

        return new SeguroResponseDto
        {
            Id = seguro.Id,
            TaxaRisco = seguro.TaxaRisco,
            PremioRisco = seguro.PremioRisco,
            PremioPuro = seguro.PremioPuro,
            PremioComercial = seguro.PremioComercial,
            Veiculo = new VeiculoDto
            {
                Id = seguro.Veiculo.Id,
                Valor = seguro.Veiculo.Valor,
                MarcaModelo = seguro.Veiculo.MarcaModelo
            },
            Segurado = new SeguradoDto
            {
                Id = seguro.Segurado.Id,
                Nome = seguro.Segurado.Nome,
                CPF = seguro.Segurado.CPF,
                Idade = seguro.Segurado.Idade
            }
        };
    }

    public async Task<RelatorioMediasDto> GerarRelatorioMediasAsync()
    {
        var seguros = await _seguroRepository.GetAllWithDetailsAsync();

        if (!seguros.Any())
        {
            return new RelatorioMediasDto
            {
                MediaValorVeiculo = 0,
                MediaTaxaRisco = 0,
                MediaPremioRisco = 0,
                MediaPremioPuro = 0,
                MediaPremioComercial = 0,
                TotalSegurosAnalisados = 0
            };
        }

        return new RelatorioMediasDto
        {
            MediaValorVeiculo = seguros.Average(s => s.Veiculo.Valor),
            MediaTaxaRisco = seguros.Average(s => s.TaxaRisco),
            MediaPremioRisco = seguros.Average(s => s.PremioRisco),
            MediaPremioPuro = seguros.Average(s => s.PremioPuro),
            MediaPremioComercial = seguros.Average(s => s.PremioComercial),
            TotalSegurosAnalisados = seguros.Count()
        };
    }

    public async Task<IEnumerable<SeguroResponseDto>> GetAllSegurosAsync()
    {
        var seguros = await _seguroRepository.GetAllAsync();
        return seguros.Select(seguro => new SeguroResponseDto
        {
            Id = seguro.Id,
            TaxaRisco = seguro.TaxaRisco,
            PremioRisco = seguro.PremioRisco,
            PremioPuro = seguro.PremioPuro,
            PremioComercial = seguro.PremioComercial,
            Veiculo = new VeiculoDto
            {
                Id = seguro.Veiculo.Id,
                Valor = seguro.Veiculo.Valor,
                MarcaModelo = seguro.Veiculo.MarcaModelo
            },
            Segurado = new SeguradoDto
            {
                Id = seguro.Segurado.Id,
                Nome = seguro.Segurado.Nome,
                CPF = seguro.Segurado.CPF,
                Idade = seguro.Segurado.Idade
            }
        }).ToList();
    }

    public async Task AtualizarValorVeiculoDoSeguroAsync(Guid seguroId, decimal novoValorVeiculo)
    {
        var seguro = await _seguroRepository.GetByIdWithDetailsAsync(seguroId);
        if (seguro == null)
        {
            //throw new KeyNotFoundException($"Seguro com Id {seguroId} não encontrado.");
            DomainExceptionValidation.When(true, $"Seguro com Id {seguroId} não encontrado.");
        }

        // A lógica de negócio para atualizar e recalcular está na entidade de domínio
        seguro.AtualizarValorVeiculo(novoValorVeiculo);

        await _seguroRepository.UpdateAsync(seguro);
    }
}
