using Application.Dtos;

namespace Application.Services;

public interface ISeguroApplicationService
{
    Task<SeguroResponseDto> RegistrarNovoSeguroAsync(decimal valorVeiculo, string marcaModeloVeiculo,
                                                    string nomeSegurado, string cpfSegurado, int idadeSegurado);
    Task<SeguroResponseDto> ObterSeguroPorIdAsync(Guid id);
    Task AtualizarValorVeiculoDoSeguroAsync(Guid seguroId, decimal novoValorVeiculo);
    Task<RelatorioMediasDto> GerarRelatorioMediasAsync();

    Task<IEnumerable<SeguroResponseDto>> GetAllSegurosAsync();
}
