using Infrastructure.Dtos;

namespace Application.Services;

public interface ISeguradoService
{
    Task<SeguradoDto> ObterSeguradoPorIdAsync(Guid id);
}