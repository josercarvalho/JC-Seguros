namespace Application.Requests;

public class CriarSeguroRequest
{
    public decimal ValorVeiculo { get; set; }
    public string MarcaModeloVeiculo { get; set; }
    public string NomeSegurado { get; set; }
    public string CpfSegurado { get; set; }
    public int IdadeSegurado { get; set; }
}
