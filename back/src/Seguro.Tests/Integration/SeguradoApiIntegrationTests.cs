using Application.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Seguro.Tests.Integration;

public class SeguradoApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>> 
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public SeguradoApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = _factory.CreateClient();
        _httpClient.BaseAddress = new Uri("http://localhost:3000"); 
    }

    [Fact]
    public async Task ObterSeguradoPorId_DeveRetornarSeguradoDoMock()
    {
        // Arrange
        var seguradoId = Guid.Parse("a1b2c3d4-e5f6-7890-1234-567890abcdef"); 
        var seguradoService = new SeguradoApiService(_httpClient); 

        // Act
        var segurado = await seguradoService.ObterSeguradoPorIdAsync(seguradoId);

        // Assert
        segurado.Should().NotBeNull();
        segurado.Id.Should().Be(seguradoId);
        segurado.Nome.Should().Be("João Silva");
        segurado.CPF.Should().Be("123.456.789-00");
    }

    [Fact]
    public async Task ObterSeguradoPorId_ComIdNaoExistente_DeveRetornarNotFound()
    {
        // Arrange
        var seguradoId = Guid.NewGuid(); 
        var seguradoService = new SeguradoApiService(_httpClient);

        // Act
        Func<Task> act = async () => await seguradoService.ObterSeguradoPorIdAsync(seguradoId);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>()
                 .Where(e => e.StatusCode == System.Net.HttpStatusCode.NotFound);
    }
}
