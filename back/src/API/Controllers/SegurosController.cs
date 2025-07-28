using Application.Requests;
using Application.Validators;
using Application.Dtos;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using SeguroResponseDto = Application.Dtos.SeguroResponseDto;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SegurosController : ControllerBase
{
    private readonly SeguroApplicationService _seguroApplicationService;

    public SegurosController(SeguroApplicationService seguroApplicationService)
    {
        _seguroApplicationService = seguroApplicationService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SeguroResponseDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllSeguros()
    {
        var seguros = await _seguroApplicationService.GetAllSegurosAsync();
        return Ok(seguros);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SeguroResponseDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> RegistrarNovoSeguro([FromBody] CriarSeguroRequest request)
    {
        try
        {
            var seguroResponse = await _seguroApplicationService.RegistrarNovoSeguroAsync(
                request.ValorVeiculo,
                request.MarcaModeloVeiculo,
                request.NomeSegurado,
                request.CpfSegurado,
                request.IdadeSegurado
            );
            return StatusCode((int)HttpStatusCode.Created, seguroResponse);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (DomainExceptionValidation ex)
        {
            return BadRequest(new { message = ex.Message, errors = ex.Errors });
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Ocorreu um erro interno no servidor.", details = ex.Message });
        }
        
    }   

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SeguroResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetSeguroById(Guid id)
    {
        try
        {
            var seguro = await _seguroApplicationService.ObterSeguroPorIdAsync(id);
            return Ok(seguro);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno ao processar sua requisição." });
        }
    }

    [HttpGet("relatorio/medias")]
    [ProducesResponseType(typeof(RelatorioMediasDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetRelatorioMedias()
    {
        var relatorio = await _seguroApplicationService.GerarRelatorioMediasAsync();
        return Ok(relatorio);
    }
}