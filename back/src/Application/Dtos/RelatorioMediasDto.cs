namespace Application.Dtos;

public class RelatorioMediasDto
{
    public decimal MediaValorVeiculo { get; set; }
    public decimal MediaTaxaRisco { get; set; }
    public decimal MediaPremioRisco { get; set; }
    public decimal MediaPremioPuro { get; set; }
    public decimal MediaPremioComercial { get; set; }
    public int TotalSegurosAnalisados { get; set; }
}
