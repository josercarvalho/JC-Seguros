using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Services;
using NSubstitute;

namespace Tests;

[TestFixture] // Indica que esta classe contém testes NUnit
public class SeguroApplicationServiceTests
{
    private SeguroApplicationService _sut; // System Under Test (o serviço que estamos testando)
    private ISeguroRepository _mockSeguroRepository; // Mock do repositório

    // [SetUp] é executado antes de cada método de teste
    [SetUp]
    public void Setup()
    {
        // Crie um mock do repositório
        _mockSeguroRepository = Substitute.For<ISeguroRepository>();
        // Instancie o serviço injetando o mock
        _sut = new SeguroApplicationService(_mockSeguroRepository);
    }

    // --- Testes para RegistrarNovoSeguroAsync ---

    [Test]
    public async Task RegistrarNovoSeguroAsync_ComDadosValidos_DeveChamarAddERetornarDtoCorreto()
    {
        // Arrange
        var valorVeiculo = 50000m;
        var marcaModeloVeiculo = "Ford Ka";
        var nomeSegurado = "João Silva";
        var cpfSegurado = "12345678900";
        var idadeSegurado = 30;

        _mockSeguroRepository.AddAsync(Arg.Any<Domain.Entities.Seguro>()).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.RegistrarNovoSeguroAsync(
            valorVeiculo, marcaModeloVeiculo, nomeSegurado, cpfSegurado, idadeSegurado);

        // Assert
        // Use FluentAssertions para asserções mais expressivas
        result.Should().NotBeNull();
        result.Veiculo.Valor.Should().Be(valorVeiculo);
        result.Segurado.Nome.Should().Be(nomeSegurado);
        result.PremioComercial.Should().BeGreaterThan(0); // Garante que o cálculo ocorreu
        result.Id.Should().NotBeEmpty();

        // Verifique se o método AddAsync no mock foi chamado exatamente uma vez
        await _mockSeguroRepository.Received(1).AddAsync(Arg.Any<Domain.Entities.Seguro>());
    }

    //[Test]
    //public async Task RegistrarNovoSeguroAsync_ComValorVeiculoInvalido_DeveLancarArgumentException()
    //{
    //    // Arrange
    //    var valorVeiculo = 0m; // Valor inválido
    //    var marcaModeloVeiculo = "Ford Ka";
    //    var nomeSegurado = "João Silva";
    //    var cpfSegurado = "12345678900";
    //    var idadeSegurado = 30;

    //    // Act & Assert
    //    // NUnit: Use Assert.ThrowsAsync para testar exceções assíncronas
    //    var ex = await Assert.ThrowsAsync<DomainExceptionValidation>(async () =>
    //    {
    //        await _sut.RegistrarNovoSeguroAsync(valorVeiculo, marcaModeloVeiculo, nomeSegurado, cpfSegurado, idadeSegurado);
    //    });

    //    ex.Message.Should().Contain("Valor do veículo deve ser maior que zero.");

    //    // Verifique se o repositório NUNCA foi chamado (a validação deve ocorrer antes)
    //    await _mockSeguroRepository.DidNotReceive().AddAsync(Arg.Any<Domain.Entities.Seguro>());
    //}

    // --- Testes para ObterSeguroPorIdAsync ---

    [Test]
    public async Task ObterSeguroPorIdAsync_ComIdExistente_DeveRetornarSeguroDto()
    {
        // Arrange
        var seguroId = Guid.NewGuid();
        var veiculo = Veiculo.CriarVeiculo(50000m, "Carro Teste");
        var segurado = Segurado.CriarSegurado("Segurado Teste", "00000000000", 25);
        var seguro = Domain.Entities.Seguro.CriarSeguro(veiculo, segurado); // Cria uma instância da entidade

        // Configure o mock para retornar a entidade quando GetByIdWithDetailsAsync for chamado com o ID específico
        _mockSeguroRepository.GetByIdWithDetailsAsync(seguroId).Returns(seguro);

        // Act
        var result = await _sut.ObterSeguroPorIdAsync(seguroId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(seguroId);
        result.Segurado.Nome.Should().Be(segurado.Nome);
        result.Veiculo.MarcaModelo.Should().Be(veiculo.MarcaModelo);

        await _mockSeguroRepository.Received(1).GetByIdWithDetailsAsync(seguroId);
    }

    //[Test]
    //public async Task ObterSeguroPorIdAsync_ComIdNaoExistente_DeveLancarKeyNotFoundException()
    //{
    //    // Arrange
    //    var seguroId = Guid.NewGuid();

    //    // Configure o mock para retornar null quando o ID não for encontrado
    //    _mockSeguroRepository.GetByIdWithDetailsAsync(seguroId).Returns((Domain.Entities.Seguro)null);

    //    // Act & Assert
    //    //var ex = await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
    //    //{
    //    //    await _sut.ObterSeguroPorIdAsync(seguroId);
    //    //});
    //    var ex = await Assert.ThrowsAsync<DomainExceptionValidation>(async () =>
    //    {
    //        await _sut.ObterSeguroPorIdAsync(seguroId);
    //    });

    //    ex.Message.Should().Contain($"Seguro com Id {seguroId} não encontrado.");

    //    await _mockSeguroRepository.Received(1).GetByIdWithDetailsAsync(seguroId);
    //}

    // --- Testes para AtualizarValorVeiculoDoSeguroAsync ---

    [Test]
    public async Task AtualizarValorVeiculoDoSeguroAsync_ComDadosValidos_DeveChamarUpdate()
    {
        // Arrange
        var seguroId = Guid.NewGuid();
        var novoValor = 75000m;

        var veiculoOriginal = Veiculo.CriarVeiculo(50000m, "Carro Original");
        var seguradoOriginal = Segurado.CriarSegurado("Nome Segurado", "11111111111", 40);
        var seguroOriginal = Seguro.CriarSeguro(veiculoOriginal, seguradoOriginal);

        _mockSeguroRepository.GetByIdWithDetailsAsync(seguroId).Returns(seguroOriginal);
        _mockSeguroRepository.UpdateAsync(Arg.Any<Seguro>()).Returns(Task.CompletedTask);

        // Act
        await _sut.AtualizarValorVeiculoDoSeguroAsync(seguroId, novoValor);

        // Assert
        // Verifique se o método UpdateAsync foi chamado
        await _mockSeguroRepository.Received(1).UpdateAsync(Arg.Is<Seguro>(s =>
            s.Id == seguroId && s.Veiculo.Valor == novoValor));

        seguroOriginal.Veiculo.Valor.Should().Be(novoValor);
        seguroOriginal.PremioComercial.Should().BeGreaterThan(0); // Deve ser recalculado
    }

    //[Test]
    //public async Task AtualizarValorVeiculoDoSeguroAsync_SeguroNaoEncontrado_DeveLancarKeyNotFoundException()
    //{
    //    // Arrange
    //    var seguroId = Guid.NewGuid();
    //    var novoValor = 75000m;

    //    _mockSeguroRepository.GetByIdWithDetailsAsync(seguroId).Returns((Domain.Entities.Seguro)null);

    //    // Act & Assert
    //    var ex = await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
    //    {
    //        await _sut.AtualizarValorVeiculoDoSeguroAsync(seguroId, novoValor);
    //    });

    //    ex.Message.Should().Contain($"Seguro com Id {seguroId} não encontrado.");

    //    await _mockSeguroRepository.DidNotReceive().UpdateAsync(Arg.Any<Domain.Entities.Seguro>());
    //}

    //[Test]
    //public async Task AtualizarValorVeiculoDoSeguroAsync_ComValorInvalido_DeveLancarDomainExceptionValidation()
    //{
    //    // Arrange
    //    var seguroId = Guid.NewGuid();
    //    var valorInvalido = 0m; // Valor inválido

    //    var veiculoOriginal = Veiculo.CriarVeiculo(50000m, "Carro Teste");
    //    var seguradoOriginal = Segurado.CriarSegurado("Nome Segurado", "11111111111", 40);
    //    var seguroOriginal = Domain.Entities.Seguro.CriarSeguro(veiculoOriginal, seguradoOriginal);

    //    _mockSeguroRepository.GetByIdWithDetailsAsync(seguroId).Returns(seguroOriginal);

    //    // Act & Assert
    //    var ex = await Assert.ThrowsAsync<DomainExceptionValidation>(async () =>
    //    {
    //        await _sut.AtualizarValorVeiculoDoSeguroAsync(seguroId, valorInvalido);
    //    });

    //    ex.Message.Should().Contain("O valor do veículo deve ser maior que zero.");

    //    await _mockSeguroRepository.DidNotReceive().UpdateAsync(Arg.Any<Domain.Entities.Seguro>());
    //}

    // --- Testes para GetAllSegurosAsync ---

    [Test]
    public async Task GetAllSegurosAsync_DeveRetornarListaDeSeguros()
    {
        // Arrange
        var veiculo1 = Veiculo.CriarVeiculo(50000m, "Modelo A");
        var segurado1 = Segurado.CriarSegurado("Pessoa A", "11111111111", 30);
        var seguro1 = Seguro.CriarSeguro(veiculo1, segurado1);

        var veiculo2 = Veiculo.CriarVeiculo(70000m, "Modelo B");
        var segurado2 = Segurado.CriarSegurado("Pessoa B", "22222222222", 40);
        var seguro2 = Seguro.CriarSeguro(veiculo2, segurado2);

        var segurosList = new List<Seguro> { seguro1, seguro2 };

        _mockSeguroRepository.GetAllAsync().Returns(segurosList);

        // Act
        var result = await _sut.GetAllSegurosAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Id.Should().Be(seguro1.Id);
        result.Last().Segurado.Nome.Should().Be(segurado2.Nome);

        await _mockSeguroRepository.Received(1).GetAllAsync();
    }

    [Test]
    public async Task GetAllSegurosAsync_QuandoNenhumSeguro_DeveRetornarListaVazia()
    {
        // Arrange
        _mockSeguroRepository.GetAllAsync().Returns(new List<Seguro>());

        // Act
        var result = await _sut.GetAllSegurosAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();

        await _mockSeguroRepository.Received(1).GetAllAsync();
    }

    [Test]
    public async Task GerarRelatorioMediasAsync_ComSegurosExistentes_DeveCalcularMediasCorretamente()
    {
        // Arrange
        var s1_v = Veiculo.CriarVeiculo(50000m, "A"); var s1_s = Segurado.CriarSegurado("X", "1", 30); var s1 = Seguro.CriarSeguro(s1_v, s1_s);
        var s2_v = Veiculo.CriarVeiculo(100000m, "B"); var s2_s = Segurado.CriarSegurado("Y", "2", 40); var s2 = Seguro.CriarSeguro(s2_v, s2_s);
        var s3_v = Veiculo.CriarVeiculo(150000m, "C"); var s3_s = Segurado.CriarSegurado("Z", "3", 50); var s3 = Seguro.CriarSeguro(s3_v, s3_s);

        var segurosList = new List<Seguro> { s1, s2, s3 };
        _mockSeguroRepository.GetAllAsync().Returns(segurosList);

        // Calcula as médias esperadas manualmente para asserção
        var expectedMediaValorVeiculo = segurosList.Average(s => s.Veiculo.Valor);
        var expectedMediaTaxaRisco = segurosList.Average(s => s.TaxaRisco);
        var expectedMediaPremioRisco = segurosList.Average(s => s.PremioRisco);
        var expectedMediaPremioPuro = segurosList.Average(s => s.PremioPuro);
        var expectedMediaPremioComercial = segurosList.Average(s => s.PremioComercial);
        var expectedTotalSeguros = segurosList.Count;

        // Act
        var result = await _sut.GerarRelatorioMediasAsync();

        // Assert
        result.Should().NotBeNull();
        result.MediaValorVeiculo.Should().BeApproximately(expectedMediaValorVeiculo, 0.001m);
        result.MediaTaxaRisco.Should().BeApproximately(expectedMediaTaxaRisco, 0.001m);
        result.MediaPremioRisco.Should().BeApproximately(expectedMediaPremioRisco, 0.001m);
        result.MediaPremioPuro.Should().BeApproximately(expectedMediaPremioPuro, 0.001m);
        result.MediaPremioComercial.Should().BeApproximately(expectedMediaPremioComercial, 0.001m);
        result.TotalSegurosAnalisados.Should().Be(expectedTotalSeguros);

        await _mockSeguroRepository.Received(1).GetAllAsync();
    }

    [Test]
    public async Task GerarRelatorioMediasAsync_QuandoNenhumSeguro_DeveRetornarRelatorioZero()
    {
        // Arrange
        _mockSeguroRepository.GetAllAsync().Returns(new List<Seguro>());

        // Act
        var result = await _sut.GerarRelatorioMediasAsync();

        // Assert
        result.Should().NotBeNull();
        result.MediaValorVeiculo.Should().Be(0);
        result.MediaTaxaRisco.Should().Be(0);
        result.MediaPremioRisco.Should().Be(0);
        result.MediaPremioPuro.Should().Be(0);
        result.MediaPremioComercial.Should().Be(0);
        result.TotalSegurosAnalisados.Should().Be(0);

        await _mockSeguroRepository.Received(1).GetAllAsync();
    }
}
