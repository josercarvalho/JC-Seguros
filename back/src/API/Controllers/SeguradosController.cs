using Application.Requests;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeguradosController : ControllerBase
{
    private readonly SeguradoApplicationService _seguradoService;

    public SeguradosController(SeguradoApplicationService seguradoService)
    {
        _seguradoService = seguradoService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var segurado = await _seguradoService.GetSeguradoByIdAsync(id);
        if (segurado == null)
        {
            return NotFound();
        }
        return Ok(segurado);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] SeguradoRequest request)
    {
        try
        {
            var seguradoId = await _seguradoService.CreateSeguradoAsync(request.Nome, request.CPF, request.Idade);
            return CreatedAtAction(nameof(GetById), new { id = seguradoId }, new { id = seguradoId });
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
            return StatusCode(500, new { message = "Ocorreu um erro interno ao criar o segurado." });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] SeguradoRequest request)
    {
        try
        {
            await _seguradoService.UpdateSeguradoAsync(id, request.Nome, request.CPF, request.Idade);
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
            return StatusCode(500, new { message = "Ocorreu um erro interno ao atualizar o segurado." });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _seguradoService.DeleteSeguradoAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocorreu um erro interno ao excluir o segurado." });
        }
    }
}