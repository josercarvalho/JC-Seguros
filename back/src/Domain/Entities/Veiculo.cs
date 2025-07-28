namespace Domain.Entities;

public class Veiculo : Entity
{
    public decimal Valor { get; private set; }
    public string MarcaModelo { get; private set; }

    private Veiculo() { }

    public static Veiculo CriarVeiculo(decimal valor, string marcaModelo)
    {
        if (valor <= 0)
            throw new ArgumentOutOfRangeException(nameof(valor), "O valor do veículo deve ser maior que zero.");
        if (string.IsNullOrWhiteSpace(marcaModelo))
            throw new ArgumentException("A marca/modelo do veículo é obrigatória.", nameof(marcaModelo));

        return new Veiculo
        {
            Valor = valor,
            MarcaModelo = marcaModelo
        };
    }

    public void AtualizarDados(decimal novoValor, string novaMarcaModelo)
    {
        if (novoValor <= 0)
            throw new ArgumentOutOfRangeException(nameof(novoValor), "O valor do veículo deve ser maior que zero.");
        if (string.IsNullOrWhiteSpace(novaMarcaModelo))
            throw new ArgumentException("A marca/modelo do veículo é obrigatória.", nameof(novaMarcaModelo));

        Valor = novoValor;
        MarcaModelo = novaMarcaModelo;
    }
}
