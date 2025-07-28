namespace Domain.Entities;

public class Seguro : Entity 
{
    public decimal ValorVeiculo { get; private set; }
    public decimal TaxaRisco { get; private set; }
    public decimal PremioRisco { get; private set; }
    public decimal PremioPuro { get; private set; }
    public decimal PremioComercial { get; private set; }

    public Guid VeiculoId { get; private set; } 
    public Veiculo Veiculo { get; private set; } 

    public Guid SeguradoId { get; private set; }
    public Segurado Segurado { get; private set; }

    private const decimal MARGEM_SEGURANCA = 0.03m;
    private const decimal LUCRO = 0.05m; 

    private Seguro() : base() { }

    public static Seguro CriarSeguro(Veiculo veiculo, Segurado segurado, decimal valorVeiculo)
    {
        if (valorVeiculo <= 0)
            throw new ArgumentOutOfRangeException(nameof(valorVeiculo), "O valor do veículo deve ser maior que zero.");
        if (veiculo == null) 
            throw new ArgumentNullException(nameof(veiculo), "O veículo é obrigatório para criar um seguro.");
        if (segurado == null) 
            throw new ArgumentNullException(nameof(segurado), "O segurado é obrigatório para criar um seguro.");

        var seguro = new Seguro
        {
            VeiculoId = veiculo.Id,
            Veiculo = veiculo,
            SeguradoId = segurado.Id,
            Segurado = segurado,
            ValorVeiculo = valorVeiculo
        };
        seguro.Calcular();
        return seguro;
    }

    public void Recalcular()
    {
        Calcular();
    }

    public void AtualizarValorVeiculo(decimal novoValorVeiculo)
    {
        if (novoValorVeiculo <= 0)
            throw new ArgumentOutOfRangeException(nameof(novoValorVeiculo), "O valor do veículo deve ser maior que zero.");

        ValorVeiculo = novoValorVeiculo;
        Calcular();
    }

    private void Calcular()
    {
        if (Veiculo == null) 
            throw new InvalidOperationException("Não é possível calcular o seguro sem um veículo associado.");
        if (Veiculo.Valor <= 0) 
            throw new InvalidOperationException("O valor do veículo deve ser maior que zero para calcular o seguro.");

        TaxaRisco = (Veiculo.Valor * 5) / (2 * Veiculo.Valor);
        PremioRisco = TaxaRisco * Veiculo.Valor;
        PremioPuro = PremioRisco * (1m + MARGEM_SEGURANCA);
        PremioComercial = PremioPuro * (1m + LUCRO); 
    }

    public static Seguro CriarSeguro(Veiculo veiculo, Segurado segurado)
    {
        if (veiculo == null)
            throw new ArgumentNullException(nameof(veiculo), "O veículo é obrigatório para criar um seguro.");
        if (segurado == null)
            throw new ArgumentNullException(nameof(segurado), "O segurado é obrigatório para criar um seguro.");

        var seguro = new Seguro
        {
            VeiculoId = veiculo.Id,
            Veiculo = veiculo,
            SeguradoId = segurado.Id,
            Segurado = segurado
        };
        seguro.Calcular();
        return seguro;
    }
}
