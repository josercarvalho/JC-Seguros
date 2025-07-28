namespace Application.Dtos;

public class SeguroDetalheDto
{
    public Guid Id { get; set; }
    public decimal ValorVeiculo { get; set; }
    public string MarcaModeloVeiculo { get; set; }
    public string NomeSegurado { get; set; }
    public string CpfSegurado { get; set; }
    public int IdadeSegurado { get; set; }
    public decimal TaxaRisco { get; set; }
    public decimal PremioRisco { get; set; }
    public decimal PremioPuro { get; set; }
    public decimal PremioComercial { get; set; }
}
