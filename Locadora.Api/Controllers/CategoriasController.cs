using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Exceptions;
using Locadora.Api.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.Api.Controllers;

/// <summary>
/// Controller para gerenciar operações CRUD de Categorias de Veículos
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _service;

    public CategoriasController(ICategoriaService service) => _service = service;

    /// <summary>
    /// Obtém todas as categorias cadastradas
    /// </summary>
    /// <returns>Lista de categorias</returns>
    /// <response code="200">Retorna a lista de categorias</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Categoria>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var categorias = await _service.ObterTodosAsync();
        return Ok(categorias);
    }

    /// <summary>
    /// Obtém uma categoria específica pelo ID
    /// </summary>
    /// <param name="id">ID da categoria</param>
    /// <returns>Dados da categoria</returns>
    /// <response code="200">Retorna a categoria encontrada</response>
    /// <response code="404">Categoria não encontrada</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Categoria), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var categoria = await _service.ObterPorIdAsync(id);
            return Ok(categoria);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { mensagem = "Categoria não encontrada" });
        }
    }

    /// <summary>
    /// Cria uma nova categoria
    /// </summary>
    /// <param name="categoria">Dados da categoria a ser criada</param>
    /// <returns>Categoria criada</returns>
    /// <response code="201">Categoria criada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost]
    [ProducesResponseType(typeof(Categoria), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] Categoria categoria)
    {
        try
        {
            await _service.AdicionarAsync(categoria);
            return CreatedAtAction(nameof(GetById), new { id = categoria.Id }, categoria);
        }
        catch (ValidacaoException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza uma categoria existente
    /// </summary>
    /// <param name="id">ID da categoria</param>
    /// <param name="categoria">Dados atualizados da categoria</param>
    /// <returns>Sem conteúdo</returns>
    /// <response code="204">Categoria atualizada com sucesso</response>
    /// <response code="400">Dados inválidos ou ID não corresponde</response>
    /// <response code="404">Categoria não encontrada</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int id, [FromBody] Categoria categoria)
    {
        if (id != categoria.Id)
            return BadRequest(new { mensagem = "O id da rota deve ser igual ao id do corpo." });

        try
        {
            await _service.ObterPorIdAsync(id);
            await _service.AtualizarAsync(categoria);
            return NoContent();
        }
        catch (ValidacaoException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { mensagem = "Categoria não encontrada" });
        }
    }

    /// <summary>
    /// Deleta uma categoria existente
    /// </summary>
    /// <param name="id">ID da categoria a ser deletada</param>
    /// <returns>Sem conteúdo</returns>
    /// <response code="204">Categoria deletada com sucesso</response>
    /// <response code="404">Categoria não encontrada</response>
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
            return NotFound(new { mensagem = "Categoria não encontrada" });
        }
    }
}
