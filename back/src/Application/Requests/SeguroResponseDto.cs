using Application.Dtos;

namespace Application.Requests;

public class SeguroResponseDto
{
    public Guid Id { get; set; }
    public decimal TaxaRisco { get; set; }
    public decimal PremioRisco { get; set; }
    public decimal PremioPuro { get; set; }
    public decimal PremioComercial { get; set; }

    public VeiculoDto Veiculo { get; set; } 
    public SeguradoDto Segurado { get; set; } 
}
