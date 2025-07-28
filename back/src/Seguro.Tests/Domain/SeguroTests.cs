using FluentAssertions;
using Domain.Entities;

namespace Seguro.Tests.Unit.Domain
{
    public class SeguroTests
    {
        [Fact]
        public void CriarSeguro_ComDadosValidos_DeveCalcularValoresCorretamente()
        {
            // Arrange
            var valorVeiculo = 50000m;
            var veiculo = Veiculo.CriarVeiculo(valorVeiculo, "Ford Ka");
            var segurado = Segurado.CriarSegurado("João Silva", "12345678900", 30);

            // Act
            var seguro = Seguro.CriarSeguro(veiculo, segurado);

            // Assert
            seguro.Should().NotBeNull();
            seguro.Id.Should().NotBeEmpty();
            seguro.Veiculo.Should().Be(veiculo);
            seguro.Segurado.Should().Be(segurado);

            var expectedTaxaRisco = 0.025m;
            var expectedPremioRisco = expectedTaxaRisco * valorVeiculo;
            var expectedPremioPuro = expectedPremioRisco * (1m + 0.03m); 
            var expectedPremioComercial = expectedPremioPuro * (1m + 0.05m);

            seguro.TaxaRisco.Should().Be(expectedTaxaRisco);
            seguro.PremioRisco.Should().BeApproximately(expectedPremioRisco, 0.01m);
            seguro.PremioPuro.Should().BeApproximately(expectedPremioPuro, 0.01m);
            seguro.PremioComercial.Should().BeApproximately(expectedPremioComercial, 0.01m);
        }

        [Fact]
        public void CriarSeguro_ComVeiculoNulo_DeveLancarArgumentNullException()
        {
            // Arrange
            Veiculo veiculo = null;
            var segurado = Segurado.CriarSegurado("João Silva", "12345678900", 30);

            // Act & Assert
            Action act = () => Seguro.CriarSeguro(veiculo, segurado);
            act.Should().Throw<ArgumentNullException>()
               .WithMessage("*O veículo é obrigatório para criar um seguro.*");
        }

        [Fact]
        public void Recalcular_DeveAtualizarValoresComBaseNoVeiculoExistente()
        {
            // Arrange
            var veiculoInicial = Veiculo.CriarVeiculo(30000m, "Fiat Palio");
            var segurado = Segurado.CriarSegurado("Maria Souza", "09876543210", 45);
            var seguro = Seguro.CriarSeguro(veiculoInicial, segurado);

            veiculoInicial.AtualizarDados(40000m, "Fiat Palio"); 

            // Act
            seguro.Recalcular(); 

            // Assert
            var novoValorVeiculo = 40000m;
            var expectedTaxaRisco = 0.025m;
            var expectedPremioRisco = expectedTaxaRisco * novoValorVeiculo;
            var expectedPremioPuro = expectedPremioRisco * (1m + 0.03m);
            var expectedPremioComercial = expectedPremioPuro * (1m + 0.05m);

            seguro.PremioRisco.Should().BeApproximately(expectedPremioRisco, 0.01m);
            seguro.PremioPuro.Should().BeApproximately(expectedPremioPuro, 0.01m);
            seguro.PremioComercial.Should().BeApproximately(expectedPremioComercial, 0.01m);
        }
    }
}