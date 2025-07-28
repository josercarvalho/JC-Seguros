using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Services;
using Moq; 
namespace Seguro.Tests.Unit.Application
{
    public class SeguroApplicationServiceTests
    {
        private readonly Mock<ISeguroRepository> _mockSeguroRepository;
        private readonly SeguroApplicationService _seguroApplicationService;

        public SeguroApplicationServiceTests()
        {
            _mockSeguroRepository = new Mock<ISeguroRepository>();
            _seguroApplicationService = new SeguroApplicationService(_mockSeguroRepository.Object);
        }

        [Fact]
        public async Task RegistrarNovoSeguroAsync_ComDadosValidos_DeveAdicionarSeguroERetornarDtoCompleto()
        {
            // Arrange
            var valorVeiculo = 60000m;
            var marcaModeloVeiculo = "Honda Civic";
            var nomeSegurado = "Carlos Pereira";
            var cpfSegurado = "11122233344";
            var idadeSegurado = 40;

            // Configurar o mock para que AddAsync não faça nada (void, mas é Task)
            object value = _mockSeguroRepository.Setup(r => r.AddAsync(It.IsAny<Seguro>()))
                                 .Returns(Task.CompletedTask);

            // Act
            var result = await _seguroApplicationService.RegistrarNovoSeguroAsync(
                valorVeiculo, marcaModeloVeiculo, nomeSegurado, cpfSegurado, idadeSegurado);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();
            result.Veiculo.Valor.Should().Be(valorVeiculo);
            result.Segurado.Nome.Should().Be(nomeSegurado);

            // Verifica se o método AddAsync do repositório foi chamado exatamente uma vez
            _mockSeguroRepository.Verify(r => r.AddAsync(It.IsAny<Seguro>()), Times.Once);
        }

        [Fact]
        public async Task ObterSeguroPorIdAsync_SeguroExistente_DeveRetornarSeguroDto()
        {
            // Arrange
            var seguroId = Guid.NewGuid();
            var veiculo = Veiculo.CriarVeiculo(50000m, "Toyota Corolla");
            var segurado = Segurado.CriarSegurado("Julia Costa", "22233344455", 35);
            var seguroDominio = Seguro.CriarSeguro(veiculo, segurado);

            _mockSeguroRepository.Setup(r => r.GetByIdWithDetailsAsync(seguroId))
                                 .ReturnsAsync(seguroDominio);

            // Act
            var result = await _seguroApplicationService.ObterSeguroPorIdAsync(seguroId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(seguroDominio.Id);
            result.Veiculo.Valor.Should().Be(seguroDominio.Veiculo.Valor);
            result.Segurado.Nome.Should().Be(segurado.Nome);
            _mockSeguroRepository.Verify(r => r.GetByIdWithDetailsAsync(seguroId), Times.Once);
        }

        [Fact]
        public async Task ObterSeguroPorIdAsync_SeguroNaoExistente_DeveRetornarNull()
        {
            // Arrange
            var seguroId = Guid.NewGuid();
            _mockSeguroRepository.Setup(r => r.GetByIdWithDetailsAsync(seguroId))
                                 .ReturnsAsync((Seguro)null);

            // Act
            var result = await _seguroApplicationService.ObterSeguroPorIdAsync(seguroId);

            // Assert
            result.Should().BeNull();
            _mockSeguroRepository.Verify(r => r.GetByIdWithDetailsAsync(seguroId), Times.Once);
        }

        [Fact]
        public async Task GerarRelatorioMediasAsync_ComSeguros_DeveCalcularMediasCorretamente()
        {
            // Arrange
            var seguros = new List<Seguro>
            {
                CriarSeguro(Veiculo.CriarVeiculo(10000m, "Car1"), Segurado.CriarSegurado("Seg1", "1", 20)),
                CriarSeguro(Veiculo.CriarVeiculo(20000m, "Car2"), Segurado.CriarSegurado("Seg2", "2", 30)),
                CriarSeguro(Veiculo.CriarVeiculo(30000m, "Car3"), Segurado.CriarSegurado("Seg3", "3", 40))
            };

            _mockSeguroRepository.Setup(r => r.GetAllWithDetailsAsync()).ReturnsAsync(seguros);

            // Act
            var result = await _seguroApplicationService.GerarRelatorioMediasAsync();

            // Assert
            result.Should().NotBeNull();
            result.MediaValorVeiculo.Should().Be(20000m); // (10000 + 20000 + 30000) / 3
            result.MediaTaxaRisco.Should().Be(0.025m); // TaxaRisco é fixa
            result.MediaPremioRisco.Should().BeApproximately(500m, 0.01m); // (250+500+750)/3
            result.MediaPremioPuro.Should().BeApproximately(515m, 0.01m);
            result.MediaPremioComercial.Should().BeApproximately(540.75m, 0.01m);
            result.TotalSegurosAnalisados.Should().Be(3);
            _mockSeguroRepository.Verify(r => r.GetAllWithDetailsAsync(), Times.Once);
        }

        [Fact]
        public async Task GerarRelatorioMediasAsync_SemSeguros_DeveRetornarMediasZero()
        {
            // Arrange
            _mockSeguroRepository.Setup(r => r.GetAllWithDetailsAsync()).ReturnsAsync(new List<Seguro>());

            // Act
            var result = await _seguroApplicationService.GerarRelatorioMediasAsync();

            // Assert
            result.Should().NotBeNull();
            result.MediaValorVeiculo.Should().Be(0);
            result.TotalSegurosAnalisados.Should().Be(0);
            _mockSeguroRepository.Verify(r => r.GetAllWithDetailsAsync(), Times.Once);
        }
    }
}