using FluentAssertions;
using Domain.Entities;

namespace Seguro.Tests.Unit.Domain
{
    public class VeiculoTests
    {
        [Fact]
        public void CriarVeiculo_ComDadosValidos_DeveRetornarVeiculo()
        {
            // Arrange
            var valor = 25000m;
            var marcaModelo = "Chevrolet Onix";

            // Act
            var veiculo = Veiculo.CriarVeiculo(valor, marcaModelo);

            // Assert
            veiculo.Should().NotBeNull();
            veiculo.Id.Should().NotBeEmpty();
            veiculo.Valor.Should().Be(valor);
            veiculo.MarcaModelo.Should().Be(marcaModelo);
        }

        [Theory]
        [InlineData(0, "Marca")]
        [InlineData(-100, "Marca")]
        public void CriarVeiculo_ComValorInvalido_DeveLancarArgumentOutOfRangeException(decimal valor, string marcaModelo)
        {
            // Act & Assert
            Action act = () => Veiculo.CriarVeiculo(valor, marcaModelo);
            act.Should().Throw<ArgumentOutOfRangeException>()
               .WithMessage("*O valor do veículo deve ser maior que zero.*");
        }

        [Theory]
        [InlineData(10000, null)]
        [InlineData(10000, "")]
        [InlineData(10000, " ")]
        public void CriarVeiculo_ComMarcaModeloInvalida_DeveLancarArgumentException(decimal valor, string marcaModelo)
        {
            // Act & Assert
            Action act = () => Veiculo.CriarVeiculo(valor, marcaModelo);
            act.Should().Throw<ArgumentException>()
               .WithMessage("*A marca/modelo do veículo é obrigatória.*");
        }

        [Fact]
        public void AtualizarDados_ComNovosDadosValidos_DeveAtualizarPropriedades()
        {
            // Arrange
            var veiculo = Veiculo.CriarVeiculo(20000m, "VW Gol");
            var novoValor = 22000m;
            var novaMarcaModelo = "VW Polo";

            // Act
            veiculo.AtualizarDados(novoValor, novaMarcaModelo);

            // Assert
            veiculo.Valor.Should().Be(novoValor);
            veiculo.MarcaModelo.Should().Be(novaMarcaModelo);
        }
    }
}