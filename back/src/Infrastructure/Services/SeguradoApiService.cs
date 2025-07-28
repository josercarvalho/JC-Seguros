using Infrastructure.Dtos;
using System.Net.Http.Json;

namespace Application.Services;

public class SeguradoApiService : ISeguradoService
{
    private readonly HttpClient _httpClient;

    public SeguradoApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<SeguradoDto> ObterSeguradoPorIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"/segurados/{id}");
        response.EnsureSuccessStatusCode(); // Lança exceção para 4xx/5xx
        var segurado = await response.Content.ReadFromJsonAsync<SeguradoDto>();
        return segurado;
    }
}
