using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Exceptions;
using Locadora.Api.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FabricantesController : ControllerBase
{
    private readonly IFabricanteService _service;

    public FabricantesController(IFabricanteService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var fabricantes = await _service.ObterTodosAsync();
        return Ok(fabricantes);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var fabricante = await _service.ObterPorIdAsync(id);
        return Ok(fabricante);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Fabricante fabricante)
    {
        try
        {
            await _service.AdicionarAsync(fabricante);
            return CreatedAtAction(nameof(GetById), new { id = fabricante.Id }, fabricante);
        }
        catch (ValidacaoException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Fabricante fabricante)
    {
        if (id != fabricante.Id)
            return BadRequest(new { mensagem = "O id da rota deve ser igual ao id do corpo." });

        try
        {
            await _service.ObterPorIdAsync(id);
            await _service.AtualizarAsync(fabricante);
            return NoContent();
        }
        catch (ValidacaoException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.RemoverAsync(id);
        return NoContent();
    }
}

