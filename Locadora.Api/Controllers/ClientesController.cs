using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Exceptions;
using Locadora.Api.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.Api.Controllers;

/// <summary>
/// Controller para gerenciar operações CRUD de Clientes
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _service;

    public ClientesController(IClienteService service) => _service = service;

    /// <summary>
    /// Obtém todos os clientes cadastrados
    /// </summary>
    /// <returns>Lista de clientes</returns>
    /// <response code="200">Retorna a lista de clientes</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Cliente>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var clientes = await _service.ObterTodosAsync();
        return Ok(clientes);
    }

    /// <summary>
    /// Obtém um cliente específico pelo ID
    /// </summary>
    /// <param name="id">ID do cliente</param>
    /// <returns>Dados do cliente</returns>
    /// <response code="200">Retorna o cliente encontrado</response>
    /// <response code="404">Cliente não encontrado</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Cliente), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var cliente = await _service.ObterPorIdAsync(id);
            return Ok(cliente);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { mensagem = "Cliente não encontrado" });
        }
    }

    /// <summary>
    /// Cria um novo cliente
    /// </summary>
    /// <param name="cliente">Dados do cliente a ser criado</param>
    /// <returns>Cliente criado</returns>
    /// <response code="201">Cliente criado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost]
    [ProducesResponseType(typeof(Cliente), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] Cliente cliente)
    {
        try
        {
            await _service.AdicionarAsync(cliente);
            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
        }
        catch (ValidacaoException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza um cliente existente
    /// </summary>
    /// <param name="id">ID do cliente</param>
    /// <param name="cliente">Dados atualizados do cliente</param>
    /// <returns>Sem conteúdo</returns>
    /// <response code="204">Cliente atualizado com sucesso</response>
    /// <response code="400">Dados inválidos ou ID não corresponde</response>
    /// <response code="404">Cliente não encontrado</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int id, [FromBody] Cliente cliente)
    {
        if (id != cliente.Id)
            return BadRequest(new { mensagem = "O id da rota deve ser igual ao id do corpo." });

        try
        {
            await _service.ObterPorIdAsync(id);
            await _service.AtualizarAsync(cliente);
            return NoContent();
        }
        catch (ValidacaoException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { mensagem = "Cliente não encontrado" });
        }
    }

    /// <summary>
    /// Deleta um cliente existente
    /// </summary>
    /// <param name="id">ID do cliente a ser deletado</param>
    /// <returns>Sem conteúdo</returns>
    /// <response code="204">Cliente deletado com sucesso</response>
    /// <response code="404">Cliente não encontrado</response>
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
            return NotFound(new { mensagem = "Cliente não encontrado" });
        }
    }
}

