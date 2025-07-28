namespace Domain.Entities;

public class Segurado : Entity
{
    public string Nome { get; private set; }
    public string CPF { get; private set; } 
    public int Idade { get; private set; } = 0;

    private Segurado() { }

    public static Segurado CriarSegurado(string nome, string cpf, int idade)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do segurado é obrigatório.", nameof(nome));
        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("O CPF do segurado é obrigatório.", nameof(cpf));
        if (idade <= 0) 
            throw new ArgumentOutOfRangeException(nameof(idade), "A idade do segurado deve ser maior que zero.");

        return new Segurado
        {
            Nome = nome,
            CPF = cpf,
            Idade = idade
        };
    }

    public void AtualizarDados(string novoNome, string novoCpf, int novaIdade)
    {
        if (string.IsNullOrWhiteSpace(novoNome))
            throw new ArgumentException("O nome do segurado é obrigatório.", nameof(novoNome));
        if (string.IsNullOrWhiteSpace(novoCpf))
            throw new ArgumentException("O CPF do segurado é obrigatório.", nameof(novoCpf));
        if (novaIdade <= 0)
            throw new ArgumentOutOfRangeException(nameof(novaIdade), "A idade do segurado deve ser maior que zero.");

        Nome = novoNome;
        CPF = novoCpf;
        Idade = novaIdade;
    }
}
