using FluentAssertions;
using Domain.Entities;

namespace Seguro.Tests.Unit.Domain
{
    public class SeguradoTests
    {
        [Fact]
        public void CriarSegurado_ComDadosValidos_DeveRetornarSegurado()
        {
            // Arrange
            var nome = "Ana Paula";
            var cpf = "11122233344";
            var idade = 25;

            // Act
            var segurado = Segurado.CriarSegurado(nome, cpf, idade);

            // Assert
            segurado.Should().NotBeNull();
            segurado.Id.Should().NotBeEmpty();
            segurado.Nome.Should().Be(nome);
            segurado.CPF.Should().Be(cpf);
            segurado.Idade.Should().Be(idade);
        }

        [Theory]
        [InlineData(null, "12345678900", 30)]
        [InlineData("", "12345678900", 30)]
        [InlineData(" ", "12345678900", 30)]
        public void CriarSegurado_ComNomeInvalido_DeveLancarArgumentException(string nome, string cpf, int idade)
        {
            // Act & Assert
            Action act = () => Segurado.CriarSegurado(nome, cpf, idade);
            act.Should().Throw<ArgumentException>()
               .WithMessage("*O nome do segurado é obrigatório.*");
        }

        [Theory]
        [InlineData("Nome Teste", null, 30)]
        [InlineData("Nome Teste", "", 30)]
        [InlineData("Nome Teste", " ", 30)]
        public void CriarSegurado_ComCpfInvalido_DeveLancarArgumentException(string nome, string cpf, int idade)
        {
            // Act & Assert
            Action act = () => Segurado.CriarSegurado(nome, cpf, idade);
            act.Should().Throw<ArgumentException>()
               .WithMessage("*O CPF do segurado é obrigatório.*");
        }

        [Theory]
        [InlineData("Nome Teste", "12345678900", 0)]
        [InlineData("Nome Teste", "12345678900", -5)]
        public void CriarSegurado_ComIdadeInvalida_DeveLancarArgumentOutOfRangeException(string nome, string cpf, int idade)
        {
            // Act & Assert
            Action act = () => Segurado.CriarSegurado(nome, cpf, idade);
            act.Should().Throw<ArgumentOutOfRangeException>()
               .WithMessage("*A idade do segurado deve ser maior que zero.*");
        }
    }
}
