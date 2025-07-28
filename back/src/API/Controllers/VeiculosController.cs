using Application.Requests;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VeiculosController : ControllerBase
{
    private readonly VeiculoApplicationService _veiculoService;

    public VeiculosController(VeiculoApplicationService veiculoService)
    {
        _veiculoService = veiculoService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var veiculo = await _veiculoService.GetVeiculoByIdAsync(id);
        if (veiculo == null)
        {
            return NotFound();
        }
        return Ok(veiculo);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] VeiculoRequest request)
    {
        try
        {
            var veiculoId = await _veiculoService.CreateVeiculoAsync(request.Valor, request.MarcaModelo);
            return CreatedAtAction(nameof(GetById), new { id = veiculoId }, new { id = veiculoId });
        }
        catch (ArgumentOutOfRangeException ex) 
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex) 
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno ao criar o veículo." });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] VeiculoRequest request)
    {
        try
        {
            await _veiculoService.UpdateVeiculoAsync(id, request.Valor, request.MarcaModelo);
            return NoContent(); 
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno ao atualizar o veículo." });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _veiculoService.DeleteVeiculoAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno ao excluir o veículo." });
        }
    }
}
