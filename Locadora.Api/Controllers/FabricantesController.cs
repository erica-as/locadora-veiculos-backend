using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Exceptions;
using Locadora.Api.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.Api.Controllers;

/// <summary>
/// Controller para gerenciar operações CRUD de Fabricantes (Marcas de Veículos)
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FabricantesController : ControllerBase
{
    private readonly IFabricanteService _service;

    public FabricantesController(IFabricanteService service) => _service = service;

    /// <summary>
    /// Obtém todos os fabricantes cadastrados
    /// </summary>
    /// <returns>Lista de fabricantes</returns>
    /// <response code="200">Retorna a lista de fabricantes</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Fabricante>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var fabricantes = await _service.ObterTodosAsync();
        return Ok(fabricantes);
    }

    /// <summary>
    /// Obtém um fabricante específico pelo ID
    /// </summary>
    /// <param name="id">ID do fabricante</param>
    /// <returns>Dados do fabricante</returns>
    /// <response code="200">Retorna o fabricante encontrado</response>
    /// <response code="404">Fabricante não encontrado</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Fabricante), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var fabricante = await _service.ObterPorIdAsync(id);
            return Ok(fabricante);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { mensagem = "Fabricante não encontrado" });
        }
    }

    /// <summary>
    /// Cria um novo fabricante
    /// </summary>
    /// <param name="fabricante">Dados do fabricante a ser criado</param>
    /// <returns>Fabricante criado</returns>
    /// <response code="201">Fabricante criado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost]
    [ProducesResponseType(typeof(Fabricante), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Atualiza um fabricante existente
    /// </summary>
    /// <param name="id">ID do fabricante</param>
    /// <param name="fabricante">Dados atualizados do fabricante</param>
    /// <returns>Sem conteúdo</returns>
    /// <response code="204">Fabricante atualizado com sucesso</response>
    /// <response code="400">Dados inválidos ou ID não corresponde</response>
    /// <response code="404">Fabricante não encontrado</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        catch (KeyNotFoundException)
        {
            return NotFound(new { mensagem = "Fabricante não encontrado" });
        }
    }

    /// <summary>
    /// Deleta um fabricante existente
    /// </summary>
    /// <param name="id">ID do fabricante a ser deletado</param>
    /// <returns>Sem conteúdo</returns>
    /// <response code="204">Fabricante deletado com sucesso</response>
    /// <response code="404">Fabricante não encontrado</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.RemoverAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { mensagem = "Fabricante não encontrado" });
        }
    }
}
